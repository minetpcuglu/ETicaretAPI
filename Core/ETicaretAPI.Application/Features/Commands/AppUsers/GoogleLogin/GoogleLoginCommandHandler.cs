using ETicaretAPI.Application.Abstractions.Services.Auths;
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

namespace ETicaretAPI.Application.Features.Commands.AppUsers.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        private readonly IAuthAppService _authAppService;

        public GoogleLoginCommandHandler(IAuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _authAppService.GoogleLoginAsync(request.IdToken, 30);
                return new()
                {
                    Token = token
                };
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
    }
}
