using ETicaretAPI.Application.Utilities.Security.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.GoogleLogin
{
   public class GoogleLoginCommandResponse
    {
        public AccessToken Token { get; set; } //basarılı ise token
    }
}
