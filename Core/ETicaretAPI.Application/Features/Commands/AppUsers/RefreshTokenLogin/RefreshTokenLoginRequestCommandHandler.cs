using ETicaretAPI.Application.Abstractions.Services.Auths;
using ETicaretAPI.Application.Utilities.Security.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.RefreshTokenLogin
{
    public class RefreshTokenLoginRequestCommandHandler : IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>
    {
        private readonly IAuthAppService _authAppService;

        public RefreshTokenLoginRequestCommandHandler(IAuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

        public async Task<RefreshTokenLoginCommandResponse> Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
        {
            //refresh token sahip kullanıcı varmı varsa o refresh token hala aktifmi epired olammısmı buna bakmaamız lazım eger refres token gecerliyse yeni bir access token uretilmesi gerekiyor ve yeni tokenı kullanıcya tekrardan geri dondurmemiz gerekicek
            AccessToken token = await _authAppService.RefreshTokenLoginAsync(request.RefreshToken);
            return new()
            {
                Token = token
            };
        }
    }
}
