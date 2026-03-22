using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.Modules.UserProfile.Application.Queries.GetUserHeatmap;
using VAlgo.Modules.UserProfile.Application.Queries.GetUserLanguages;
using VAlgo.Modules.UserProfile.Application.Queries.GetUserPracticeHistory;
using VAlgo.Modules.UserProfile.Application.Queries.GetUserRecentAc;
using VAlgo.Modules.UserProfile.Application.Queries.GetUserSkills;
using VAlgo.Modules.UserProfile.Application.Queries.GetUserStats;

namespace VAlgo.API.Controllers.Identity
{
    [ApiController]
    [Route("api/users")]
    public sealed class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/users/{username}/stats
        [AllowAnonymous]
        [HttpGet("{username}/stats")]
        public async Task<IActionResult> GetUserStats([FromRoute] string username, CancellationToken cancellationToken)
        {
            var query = new GetUserStatsQuery(username);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // GET api/users/{username}/heatmap
        [AllowAnonymous]
        [HttpGet("{username}/heatmap")]
        public async Task<IActionResult> GetUserHeatmap([FromRoute] string username, CancellationToken cancellationToken)
        {
            var query = new GetUserHeatmapQuery(username);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // GET api/users/{username}/recent-ac
        [AllowAnonymous]
        [HttpGet("{username}/recent-ac")]
        public async Task<IActionResult> GetUserRecentAc([FromRoute] string username, [FromQuery] int count = 10, CancellationToken cancellationToken = default)
        {
            var query = new GetUserRecentAcQuery(username, count);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // GET api/users/{username}/languages
        [AllowAnonymous]
        [HttpGet("{username}/languages")]
        public async Task<IActionResult> GetUserLanguages([FromRoute] string username, CancellationToken cancellationToken)
        {
            var query = new GetUserLanguagesQuery(username);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // GET api/users/{username}/skills
        [AllowAnonymous]
        [HttpGet("{username}/skills")]
        public async Task<IActionResult> GetUserSkills([FromRoute] string username, CancellationToken cancellationToken)
        {
            var query = new GetUserSkillsQuery(username);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // GET api/users/{username}/practice-history
        [AllowAnonymous]
        [HttpGet("{username}/practice-history")]
        public async Task<IActionResult> GetUserPracticeHistory([FromRoute] string username, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetUserPracticeHistoryQuery(username, page, pageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}