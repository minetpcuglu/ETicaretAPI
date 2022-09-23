using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.Create
{
   public class CreateAppUserCommandRequest : IRequest<CreateAppUserCommandResponse>
    {
        public string NameSurname { get; set; }
        public string Username { get; set; }
        public string PasswordConfirm { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
