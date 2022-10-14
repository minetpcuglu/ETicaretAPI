using ETicaretAPI.Application.DTOs.User;
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
    }
}
