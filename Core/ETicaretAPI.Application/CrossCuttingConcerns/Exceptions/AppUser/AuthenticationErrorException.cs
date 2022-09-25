using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.CrossCuttingConcerns.Exceptions.AppUser
{
   public class AuthenticationErrorException:Exception
    {
        public AuthenticationErrorException() : base("Kimlik dogrulama hatası")
        {

        }
        public AuthenticationErrorException(string? message) : base(message)
        {

        }
        public AuthenticationErrorException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
