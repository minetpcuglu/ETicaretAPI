using ETicaretAPI.Application.Abstractions.Services.Auths;
using ETicaretAPI.Application.CrossCuttingConcerns.Exceptions.AppUser;
using ETicaretAPI.Application.Utilities.Security.DTOs;
using ETicaretAPI.Application.Utilities.Security.Token;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.Login
{
    public class LoginAppUserCommandHandler : IRequestHandler<LoginAppUserCommandRequest, LoginAppUserCommandResponse>
    {
        private readonly IAuthAppService _authAppService;

        public LoginAppUserCommandHandler(IAuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

        public async Task<LoginAppUserCommandResponse> Handle(LoginAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authAppService.LoginAsync(request.UsernameOrEmail, request.Password,900);
            return new LoginAppUserSuccessCommandResponse()
            {
                Token = token
            };
        }
    }
}
