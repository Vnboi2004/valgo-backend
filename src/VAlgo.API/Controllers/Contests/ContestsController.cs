using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.Contests.Requests;
using VAlgo.Modules.Contests.Application.Commands.AddProblemToContest;
using VAlgo.Modules.Contests.Application.Commands.ArchiveContest;
using VAlgo.Modules.Contests.Application.Commands.CreateContest;
using VAlgo.Modules.Contests.Application.Commands.FinishContest;
using VAlgo.Modules.Contests.Application.Commands.FreezeLeaderboard;
using VAlgo.Modules.Contests.Application.Commands.JoinContest;
using VAlgo.Modules.Contests.Application.Commands.LeaveContest;
using VAlgo.Modules.Contests.Application.Commands.PublishContest;
using VAlgo.Modules.Contests.Application.Commands.RegisterContest;
using VAlgo.Modules.Contests.Application.Commands.RejudgeContest;
using VAlgo.Modules.Contests.Application.Commands.RemoveParticipant;
using VAlgo.Modules.Contests.Application.Commands.RemoveProblemFromContest;
using VAlgo.Modules.Contests.Application.Commands.ReorderContestProblems;
using VAlgo.Modules.Contests.Application.Commands.StartContest;
using VAlgo.Modules.Contests.Application.Commands.StartVirtualContest;
using VAlgo.Modules.Contests.Application.Commands.UnregisterContest;
using VAlgo.Modules.Contests.Application.Commands.UpdateContestMaxParticipants;
using VAlgo.Modules.Contests.Application.Commands.UpdateContestMetadata;
using VAlgo.Modules.Contests.Application.Commands.UpdateContestProblemPoints;
using VAlgo.Modules.Contests.Application.Commands.UpdateContestSchedule;
using VAlgo.Modules.Contests.Application.Commands.UpdateContestVisibility;
using VAlgo.Modules.Contests.Application.Queries.GetContestDetail;
using VAlgo.Modules.Contests.Application.Queries.GetContestLeaderboard;
using VAlgo.Modules.Contests.Application.Queries.GetContestMe;
using VAlgo.Modules.Contests.Application.Queries.GetContestParticipants;
using VAlgo.Modules.Contests.Application.Queries.GetContestProblems;
using VAlgo.Modules.Contests.Application.Queries.GetContests;

namespace VAlgo.API.Controllers.Contests
{
    [ApiController]
    [Route("api/contests")]
    [Authorize]
    public sealed class ContestsController : Controller
    {
        private readonly IMediator _mediator;

        public ContestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Contest CRUD
        #endregion

        // POST api/contests
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPost]
        public async Task<IActionResult> CreateContest([FromBody] CreateContestsRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateContestCommand(
                request.Title,
                request.Description,
                request.Code,
                request.StartTime,
                request.EndTime,
                request.Visibility,
                request.Type
            );

            var contestId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetContestDetail), new { contestId }, contestId);
        }

        // GET api/contests
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetContest([FromQuery] GetContestsRequest request, CancellationToken cancellationToken)
        {
            var query = new GetContestsQuery(
                request.Phase,
                request.Visibility,
                request.Page,
                request.PageSize
            );

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/contests/{contestId}
        [AllowAnonymous]
        [HttpGet("{contestId:guid}")]
        public async Task<IActionResult> GetContestDetail([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var query = new GetContestDetailQuery(contestId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        #region Contest Metadata
        #endregion 

        // PUT api/contests/{contestId}/metadata
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPatch("{contestId:guid}/metadata")]
        public async Task<IActionResult> UpdateContestMetadata([FromRoute] Guid contestId, [FromBody] UpdateContestMetadataRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateContestMetadataCommand(contestId, request.Title, request.Description);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/contests/{contestId}/schedule
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPatch("{contestId:guid}/schedule")]
        public async Task<IActionResult> UpdateContestSchedule([FromRoute] Guid contestId, [FromBody] UpdateContestScheduleRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateContestScheduleCommand(contestId, request.StartTime, request.EndTime);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/contests/{contestId}/visibility
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPatch("{contestId:guid}/visibility")]
        public async Task<IActionResult> UpdateContestVisibility([FromRoute] Guid contestId, [FromBody] UpdateContestVisibilityRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateContestVisibilityCommand(contestId, request.Visibility);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/contests/{contestId}/max-participants
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPatch("{contestId:guid}/max-participants")]
        public async Task<IActionResult> UpdateContestMaxParticipants([FromRoute] Guid contestId, [FromForm] UpdateContestMaxParticipantsRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateContestMaxParticipantsCommand(contestId, request.MaxParticipants);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        #region Contest lifecycle
        #endregion

        // POST api/contests/{contestId}/publish
        [Authorize(Roles = "Admin")]
        [HttpPost("{contestId:guid}/publish")]
        public async Task<IActionResult> PublishContest([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var command = new PublishContestCommand(contestId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/contests/{contestId}/start
        [Authorize(Roles = "Admin")]
        [HttpPost("{contestId:guid}/started")]
        public async Task<IActionResult> StartContest([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var command = new StartContestCommand(contestId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/contests/{contestId}/finish
        [Authorize(Roles = "Admin")]
        [HttpPost("{contestId:guid}/finish")]
        public async Task<IActionResult> FinishContest([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var command = new FinishContestCommand(contestId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/contests/{contestId}/archive
        [Authorize(Roles = "Admin")]
        [HttpPost("{contestId:guid}/archive")]
        public async Task<IActionResult> ArchiveContest([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var command = new ArchiveContestCommand(contestId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        #region Contest Problems
        #endregion

        // GET api/contests/{contestId}/problems
        [Authorize]
        [HttpGet("{contestId:guid}/problems")]
        public async Task<IActionResult> GetContestProblems([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var query = new GetContestProblemsQuery(contestId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // POST api/contests/{contestId}/problems
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPost("{contestId:guid}/problems")]
        public async Task<IActionResult> AddProblemToContest([FromRoute] Guid contestId, [FromBody] AddProblemToContestRequest request, CancellationToken cancellationToken)
        {
            var command = new AddProblemToContestCommand(contestId, request.ProblemId, request.Code, request.Points);

            var problemId = await _mediator.Send(command, cancellationToken);

            return Ok(problemId);
        }

        // DELETE api/contests/{contestId}/problems/{problemId}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpDelete("{contestId:guid}/problems/{problemId:guid}")]
        public async Task<IActionResult> RemoveProblemFromContest([FromRoute] Guid contestId, [FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var command = new RemoveProblemFromContestCommand(contestId, problemId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // UPDATE api/contests/{contestId}/problems/order
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{contestId:guid}/problems/order")]
        public async Task<IActionResult> ReorderContestProblems([FromRoute] Guid contestId, [FromBody] ReorderContestProblemsRequest request, CancellationToken cancellationToken)
        {
            var command = new ReorderContestProblemsCommand(contestId, request.ProblemIds);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // UPDATE api/contests/{contestId}/problems/{problemId}/points
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{contestId:guid}/problems/{problemId:guid}/points")]
        public async Task<IActionResult> UpdateContestProblemPoints([FromRoute] Guid contestId, [FromRoute] Guid problemId, [FromBody] UpdateContestProblemPointsRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateContestProblemPointsCommand(contestId, problemId, request.Points);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        #region Participants
        #endregion

        // GET api/contests/{contestId}/participants
        [HttpGet("{contestId:guid}/participants")]
        public async Task<IActionResult> GetContestParticipants([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var query = new GetContestParticipantsQuery(contestId);

            var reuslt = await _mediator.Send(query, cancellationToken);

            return Ok(reuslt);
        }

        // POST api/contests/{contestId}/participants
        [Authorize(Roles = "User")]
        [HttpPost("{contestId:guid}/participants")]
        public async Task<IActionResult> JoinContest([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var command = new JoinContestCommand(contestId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/contests/{contestId}/participants/{userId}
        [Authorize(Roles = "User")]
        [HttpDelete("{contestId:guid}/participants/me")]
        public async Task<IActionResult> LeaveContest([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var command = new LeaveContestCommand(contestId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        #region Leaderboard
        #endregion
        [HttpGet("{contestId:guid}/leaderboard")]
        public async Task<IActionResult> GetContestLeaderboard([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var query = new GetContestLeaderboardQuery(contestId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPost("{contestId:guid}/register")]
        public async Task<IActionResult> RegisterContest(Guid contestId, CancellationToken cancellationToken)
        {
            var command = new RegisterContestCommand(contestId);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpPost("{contestId:guid}/unregister")]
        public async Task<IActionResult> UnregisterContest(Guid contestId, CancellationToken cancellationToken)
        {
            var command = new UnregisterContestCommand(contestId);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpPost("{contestId:guid}/virtual/start")]
        public async Task<IActionResult> StartVirtualContest(Guid contestId, CancellationToken cancellationToken)
        {
            var command = new StartVirtualContestCommand(contestId);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpPost("{contestId:guid}/freeze")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FreezeLeaderboard(Guid contestId)
        {
            var command = new FreezeLeaderboardCommand(contestId);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("{contestId:guid}/rejudge")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejudgeContest(Guid contestId)
        {
            var command = new RejudgeContestCommand(contestId);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{contestId:guid}/participants/{userId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveParticipant(Guid contestId, Guid userId)
        {
            var command = new RemoveParticipantCommand(contestId, userId);

            await _mediator.Send(command);

            return Ok();
        }

        [Authorize]
        [HttpGet("{contestId:guid}/me")]
        public async Task<IActionResult> GetMyContestState([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var query = new GetContestMeQuery(contestId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}