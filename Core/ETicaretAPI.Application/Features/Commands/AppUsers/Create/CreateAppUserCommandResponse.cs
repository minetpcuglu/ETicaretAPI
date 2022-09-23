using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.Create
{
   public class CreateAppUserCommandResponse
    {
        public bool Succeeded { get; set; } //ekleme işlemi basarılı ise
        public string Message { get; set; } //ekleme işlemi basarılı ise
    }
}
