using ETicaretAPI.Application.Features.Commands.AppUsers.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUsers.Login;
using ETicaretAPI.Application.Features.Commands.AppUsers.RefreshTokenLogin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login(LoginAppUserCommandRequest request)
        {
            LoginAppUserCommandResponse value = await _mediator.Send(request);
            return Ok(value);
        }

        [HttpPost]
        [Route("GoogleLogin")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest request)
        {
            GoogleLoginCommandResponse value = await _mediator.Send(request);
            return Ok(value);
        }


        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenLoginCommandRequest request) //parametreyi cqrs ten alıncak 
        {
            RefreshTokenLoginCommandResponse value = await _mediator.Send(request);
            return Ok(value);
        }
    }
}
