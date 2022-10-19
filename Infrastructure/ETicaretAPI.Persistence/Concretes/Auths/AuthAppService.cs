using ETicaretAPI.Application.Abstractions.Services.Auths;
using ETicaretAPI.Application.Abstractions.Services.Users;
using ETicaretAPI.Application.CrossCuttingConcerns.Exceptions.AppUser;
using ETicaretAPI.Application.Features.Commands.AppUsers.Login;
using ETicaretAPI.Application.Utilities.Security.DTOs;
using ETicaretAPI.Application.Utilities.Security.Token;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Concretes.Auths
{
    public class AuthAppService : IAuthAppService
    {
        private readonly UserManager<AppUser> _userManager;
        public IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler; //kullanıcıya token olusturma için
        private readonly IUserAppService _userAppService;

        public AuthAppService(UserManager<AppUser> userManager, IConfiguration configuration, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler, IUserAppService userAppService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _userAppService = userAppService;
        }

        public async Task<AccessToken> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["LoginSettings:Google:Client_ID"] } //hangi proje üzerinde dogrulaam yyapılcak
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            try
            {
                bool result = user != null;
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(payload.Email);
                    if (user == null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = payload.Email,
                            UserName = payload.Email,
                            NameSurname = payload.Name
                        };
                        var identityResult = await _userManager.CreateAsync(user);
                        result = identityResult.Succeeded;
                    }
                }
                if (result)
                    await _userManager.AddLoginAsync(user, info); //AspNetUserLogins
                else
                    throw new Exception("Invalid external authentication.");

                AccessToken token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);
                await _userAppService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15); //refresh token cagırma

                return token;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<AccessToken> LoginAsync(string userNameOrEmail, string password,int accessTokenLifeTime)
        {
            AppUser user = await _userManager.FindByNameAsync(userNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(userNameOrEmail);
            if (user == null)
                throw new NotFoundUserException();
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded) //authentication başarılı
            {
                AccessToken token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user); //5 dk lık bir token olsutur
                await _userAppService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15); //refresh token cagırma
                return token;
            }         
            throw new AuthenticationErrorException();
        }

        public async Task<AccessToken> RefreshTokenLoginAsync(string refreshToken)
        {
          AppUser? user=  _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken); // refresh token degeri kuallanıcadan gelenle eşit olan varmı
            //varsa ?
            if (user!=null && user?.RefreshTokenEndDate > DateTime.Now) //refresh token acces tokenın suresi bitirlmiş o fazlalık olan suredde örn accestoekn 45 refreshtoken 60 suan 55 de oalbilşir
            {
             AccessToken token=   _tokenHandler.CreateAccessToken(15,user);
              await  _userAppService.UpdateRefreshToken(token.RefreshToken,user,token.Expiration,300); //refresh token kuallnıcadan gelen tokenla güncelle
                return token;
            }
            else
            {
                throw new NotFoundUserException();
            }
           
        }
    }
}
