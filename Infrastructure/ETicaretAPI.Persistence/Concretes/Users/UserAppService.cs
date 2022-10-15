using ETicaretAPI.Application.Abstractions.Services.Users;
using ETicaretAPI.Application.CrossCuttingConcerns.Exceptions.AppUser;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Concretes.Users
{
    public class UserAppService : IUserAppService
    {
   
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserAppService( UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUserDto createUserDto)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = createUserDto.Username,
                Email = createUserDto.Email,
                NameSurname = createUserDto.NameSurname
            }, createUserDto.Password);

            CreateUserResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = ("Kullanıcı eklendi");
            else
                foreach (var item in result.Errors)
                    response.Message += $"{item.Description} - {item.Code}\n";
            return response;
            
        }

        public async Task<bool> UpdateRefreshToken(string refreshToken, AppUser appUser, DateTime accessTokenDate,int addOnAccessTokenDate)
        {
          
            if (appUser!=null)
            {
                appUser.RefreshToken = refreshToken; //refresh token güncelle
                appUser.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);//access token üzerine 5 10 dk eklenen hali refresh token
                await _userManager.UpdateAsync(appUser);
                return true;
            }
            return false;
            throw new NotFoundUserException();
           
        }
    }
}
