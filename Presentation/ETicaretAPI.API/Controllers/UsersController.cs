using ETicaretAPI.Application.Features.Commands.AppUsers.Create;
using ETicaretAPI.Application.Features.Commands.AppUsers.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUsers.Login;
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
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CreateUser")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateUser(CreateAppUserCommandRequest request)
        {
            CreateAppUserCommandResponse value = await _mediator.Send(request);
            return Ok(value);
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
    }
}
