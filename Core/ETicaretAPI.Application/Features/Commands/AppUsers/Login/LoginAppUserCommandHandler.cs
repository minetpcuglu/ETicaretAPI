using ETicaretAPI.Application.CrossCuttingConcerns.Exceptions.AppUser;
using ETicaretAPI.Application.Utilities.Security.DTOs;
using ETicaretAPI.Application.Utilities.Security.Token;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.Login
{
    public class LoginAppUserCommandHandler : IRequestHandler<LoginAppUserCommandRequest, LoginAppUserCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;

        public LoginAppUserCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginAppUserCommandResponse> Handle(LoginAppUserCommandRequest request, CancellationToken cancellationToken)
        {
         AppUser user=  await _userManager.FindByNameAsync(request.UsernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            if (user==null)
                throw new NotFoundUserException();
            SignInResult result=  await _signInManager.CheckPasswordSignInAsync(user,request.Password,false);
            if (result.Succeeded) //authentication başarılı
            {
               AccessToken token =  _tokenHandler.CreateAccessToken(5); //5 dk lık bir token olsutur
                return new LoginAppUserSuccessCommandResponse()
                {
                    Token =token
                };
            }
            //return new LoginAppUserErrorCommandResponse()
            //{
            //    ErrorMessage = "Kullanıcı adı veya şifre hatalı.Token Alınamadı"
            //};
            throw new AuthenticationErrorException();
        }
    }
}
