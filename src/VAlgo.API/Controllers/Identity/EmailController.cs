using MediatR;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.Identity.Requests;
using VAlgo.Modules.Identity.Application.Commands.VerifyEmailUser;

namespace VAlgo.API.Controllers.Identity
{
    [ApiController]
    [Route("api/identity/email")]
    public sealed class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/identity/email/verify
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyEmailUser([FromBody] VerifyEmailUserRequest request, CancellationToken cancellationToken)
        {
            var command = new VerifyEmailUserCommand(request.Token);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}