using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.Identity.Requests;
using VAlgo.Modules.Identity.Application.Commands.LoginUser;
using VAlgo.Modules.Identity.Application.Commands.LogoutUser;
using VAlgo.Modules.Identity.Application.Commands.RefreshTokenUser;

namespace VAlgo.API.Controllers.Identity
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        // POST api/auth/refresh-token
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenUser([FromBody] RefreshTokenUserRequest request, CancellationToken cancellationToken)
        {
            var command = new RefreshTokenUserCommand(request.RefreshToken);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        // POST api/auth/logout
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser([FromBody] LogoutUserRequest request, CancellationToken cancellationToken)
        {
            var commnad = new LogoutUserCommand(request.RefreshToken);
            await _mediator.Send(commnad, cancellationToken);
            return NoContent();
        }
    }
}