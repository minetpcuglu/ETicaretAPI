using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Users
{
    public interface IUserAppService
    {
        Task<CreateUserResponse> CreateAsync(CreateUserDto createUserDto);
        Task<bool> UpdateRefreshToken(string refreshToken,AppUser appUser,DateTime accessTokenDate, int addOnAccessTokenDate);
    }
}
