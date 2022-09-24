using ETicaretAPI.Application.CrossCuttingConcerns.Exceptions.AppUser;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.Create
{
    public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommandRequest, CreateAppUserCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        public CreateAppUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateAppUserCommandResponse> Handle(CreateAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Username,
                Email = request.Email,
                NameSurname = request.NameSurname
            }, request.Password) ;

            CreateAppUserCommandResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
               response.Message=("Kullanıcı eklendi");
            else
                foreach (var item in result.Errors)
                    response.Message += $"{item.Description} - {item.Code}\n";
            return response;
            
        }
    }
}
