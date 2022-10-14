using ETicaretAPI.Application.Utilities.Security.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Auths
{
    public interface IAuthAppService
    {
        Task<AccessToken> GoogleLoginAsync(string idToken,int accessTokenLifeTime);
        Task<AccessToken> LoginAsync(string userNameOrEmail,string password, int accessTokenLifeTime);
    }
}
