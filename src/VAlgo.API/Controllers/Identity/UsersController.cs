using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.Identity.Requests;
using VAlgo.Modules.Identity.Application.Commands.UpdateAvatar;
using VAlgo.Modules.Identity.Application.Commands.UpdatePrivacySettings;
using VAlgo.Modules.Identity.Application.Commands.UpdateUserExperience;
using VAlgo.Modules.Identity.Application.Commands.UpdateUserProfile;
using VAlgo.Modules.Identity.Application.Queries.GetMyProfile;
using VAlgo.Modules.Identity.Application.Queries.GetUserPublicProfile;

namespace VAlgo.API.Controllers.Identity
{
    [ApiController]
    [Route("api/users")]
    public sealed class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/users/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
        {
            var query = new GetMyProfileQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // GET api/users/{username}
        [AllowAnonymous]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserPublicProfileQuery([FromRoute] string username, CancellationToken cancellationToken)
        {
            var query = new GetUserPublicProfileQuery(username);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // PUT api/users/me/profile
        [Authorize]
        [HttpPut("me/profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateUserProfileCommand(
                request.DisplayName,
                request.Gender,
                request.Location,
                request.Birthday,
                request.Website,
                request.Github,
                request.LinkedIn,
                request.Twitter,
                request.ReadMe
            );
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        // PUT api/users/me/experience
        [Authorize]
        [HttpPut("me/experience")]
        public async Task<IActionResult> UpdateUserExperience([FromBody] UpdateUserExperienceRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateUserExperienceCommand(request.Work, request.Education);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        // PUT api/users/me/privacy
        [Authorize]
        [HttpPut("me/privacy")]
        public async Task<IActionResult> UpdatePrivacySettings([FromBody] UpdatePrivacySettingsRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdatePrivacySettingsCommand(request.ShowRecentSubmissions, request.ShowSubmissionHeatmap);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        // PUT api/users/me/avatar
        [Authorize]
        [HttpPut("me/avatar")]
        public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateAvatarCommand(request.AvatarUrl);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}