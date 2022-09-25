using ETicaretAPI.Application.Utilities.Security.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.Login
{
    public class LoginAppUserCommandResponse
    {
    }
    //hata dönecek response
    public class LoginAppUserSuccessCommandResponse:LoginAppUserCommandResponse
    {
        public AccessToken Token { get; set; } //basarılı ise token
    }
    public class LoginAppUserErrorCommandResponse:LoginAppUserCommandResponse
    {
        public string ErrorMessage { get; set; } //başarısız ise
    }
}
