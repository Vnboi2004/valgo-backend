using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.Identity.Requests;
using VAlgo.Modules.Identity.Application.Commands.RegisterUser;
using VAlgo.Modules.Identity.Application.Commands.VerifyEmailUser;

namespace VAlgo.API.Controllers.Identity
{
    [ApiController]
    [Route("api/registration")]
    [AllowAnonymous]
    public sealed class RegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegistrationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/registration
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(
                request.Username,
                request.Email,
                request.Password
            );
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        // POST api/registration/verify-email
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmailUser([FromBody] VerifyEmailUserRequest request, CancellationToken cancellationToken)
        {
            var command = new VerifyEmailUserCommand(request.Token);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}