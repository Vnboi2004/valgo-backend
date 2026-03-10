using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.Identity.Requests;
using VAlgo.Modules.Identity.Application.Commands.ChangePasswordUser;
using VAlgo.Modules.Identity.Application.Commands.ForgotPasswordUser;
using VAlgo.Modules.Identity.Application.Commands.ResetPasswordUser;

namespace VAlgo.API.Controllers.Identity
{
    [ApiController]
    [Route("api/password")]
    public sealed class PasswordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PasswordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/password/forgot
        [HttpPost("forgot")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassswordUser([FromBody] ForgotPasswordUserRequest request, CancellationToken cancellationToken)
        {
            var command = new ForgotPasswordUserCommand(request.Email);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        // POST api/password/reset
        [HttpPost("reset")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordUser([FromBody] ResetPasswordUserRequest request, CancellationToken cancellationToken)
        {
            var command = new ResetPasswordUserCommand(request.Token, request.NewPassword);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        // POST api/password/change
        [Authorize]
        [HttpPost("change")]
        public async Task<IActionResult> ChangePasswordUser([FromBody] ChangePasswordUserRequest request, CancellationToken cancellationToken)
        {
            var command = new ChangePasswordUserCommand(
                request.UserId,
                request.CurrentPassword,
                request.NewPassword
            );

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}