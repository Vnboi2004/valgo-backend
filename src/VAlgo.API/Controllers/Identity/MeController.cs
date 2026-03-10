using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.Modules.Identity.Application.Queries.GetCurrentUser;

namespace VAlgo.API.Controllers.Identity
{
    [ApiController]
    [Route("api/me")]
    [Authorize]
    public sealed class MeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/me
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var query = new GetCurrentUserQuery(Guid.Empty);
            var user = await _mediator.Send(query, cancellationToken);
            return Ok(user);
        }
    }
}