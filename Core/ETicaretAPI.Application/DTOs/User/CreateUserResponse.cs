using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.User
{
   public class CreateUserResponse
    {
        public bool Succeeded { get; set; } //ekleme işlemi basarılı ise
        public string Message { get; set; } //ekleme işlemi basarılı ise
    }
}
