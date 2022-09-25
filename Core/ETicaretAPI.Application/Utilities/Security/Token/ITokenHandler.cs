using ETicaretAPI.Application.Utilities.Security.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Utilities.Security.Token
{
   public interface ITokenHandler
    {
        AccessToken CreateAccessToken(int minute); //token ömrünü;
    }
}
