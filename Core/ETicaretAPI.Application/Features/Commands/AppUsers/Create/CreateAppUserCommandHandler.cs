using ETicaretAPI.Application.Abstractions.Services.Users;
using ETicaretAPI.Application.CrossCuttingConcerns.Exceptions.AppUser;
using ETicaretAPI.Application.DTOs.User;
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
 
        private readonly IUserAppService _userAppService;

        public CreateAppUserCommandHandler( IUserAppService userAppService)
        {
            
            _userAppService = userAppService;
        }

        public async Task<CreateAppUserCommandResponse> Handle(CreateAppUserCommandRequest request, CancellationToken cancellationToken)
        {
          CreateUserResponse response=await  _userAppService.CreateAsync(new()
            {
                Email = request.Email,
                NameSurname = request.NameSurname,
                Username = request.Username,
                Password=request.Password,
                PasswordConfirm=request.PasswordConfirm,
            });
            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded
            };
            
        }
    }
}
