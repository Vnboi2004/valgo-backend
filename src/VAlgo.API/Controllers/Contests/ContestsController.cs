using MediatR;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.Contests.Requests;
using VAlgo.Modules.Contests.Application.Commands.AddProblemToContest;
using VAlgo.Modules.Contests.Application.Commands.ArchiveContest;
using VAlgo.Modules.Contests.Application.Commands.CreateContest;
using VAlgo.Modules.Contests.Application.Commands.JoinContest;
using VAlgo.Modules.Contests.Application.Commands.LeaveContest;
using VAlgo.Modules.Contests.Application.Commands.PublishContest;
using VAlgo.Modules.Contests.Application.Commands.RemoveProblemFromContest;
using VAlgo.Modules.Contests.Application.Commands.ReorderContestProblems;
using VAlgo.Modules.Contests.Application.Commands.StartContest;
using VAlgo.Modules.Contests.Application.Commands.UpdateContestMaxParticipants;
using VAlgo.Modules.Contests.Application.Commands.UpdateContestMetadata;
using VAlgo.Modules.Contests.Application.Commands.UpdateContestProblemPoints;
using VAlgo.Modules.Contests.Application.Commands.UpdateContestSchedule;
using VAlgo.Modules.Contests.Application.Commands.UpdateContestVisibility;
using VAlgo.Modules.Contests.Application.Queries.GetContestDetail;
using VAlgo.Modules.Contests.Application.Queries.GetContestLeaderboard;
using VAlgo.Modules.Contests.Application.Queries.GetContestParticipants;
using VAlgo.Modules.Contests.Application.Queries.GetContestProblems;
using VAlgo.Modules.Contests.Application.Queries.GetContests;

namespace VAlgo.API.Controllers.Contests
{
    [ApiController]
    [Route("api/contests")]
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
        [HttpPost]
        public async Task<IActionResult> CreateContest([FromBody] CreateContestsRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateContestCommand(
                request.Title,
                request.Description,
                request.StartTime,
                request.EndTime,
                request.Visibility,
                request.CreatedBy,
                request.MaxParticipants
            );

            var contestId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetContestDetail), new { contestId }, contestId);
        }

        // GET api/contests
        [HttpGet]
        public async Task<IActionResult> GetContest([FromQuery] GetContestsRequest request, CancellationToken cancellationToken)
        {
            var query = new GetContestsQuery(
                request.Status,
                request.Visibility,
                request.Page,
                request.PageSize
            );

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/contests/{contestId}
        [HttpGet("{contestId:guid}")]
        public async Task<IActionResult> GetContestDetail([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var query = new GetContestDetailQuery(contestId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(query);
        }

        #region Contest Metadata
        #endregion 

        // PUT api/contests/{contestId}/metadata
        [HttpPatch("{contestId:guid}/metadata")]
        public async Task<IActionResult> UpdateContestMetadata([FromRoute] Guid contestId, [FromBody] UpdateContestMetadataRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateContestMetadataCommand(contestId, request.Title, request.Description);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/contests/{contestId}/schedule
        [HttpPatch("{contestId:guid}/schedule")]
        public async Task<IActionResult> UpdateContestSchedule([FromRoute] Guid contestId, [FromBody] UpdateContestScheduleRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateContestScheduleCommand(contestId, request.StartTime, request.EndTime);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/contests/{contestId}/visibility
        [HttpPatch("{contestId:guid}/visibility")]
        public async Task<IActionResult> UpdateContestVisibility([FromRoute] Guid contestId, [FromBody] UpdateContestVisibilityRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateContestVisibilityCommand(contestId, request.Visibility);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/contests/{contestId}/max-participants
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
        [HttpPost("{contestId:guid}/publish")]
        public async Task<IActionResult> PublishContest([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var command = new PublishContestCommand(contestId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/contests/{contestId}/start
        [HttpPost("{contestId:guid}/started")]
        public async Task<IActionResult> StartContest([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var command = new StartContestCommand(contestId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/contests/{contestId}/finish
        [HttpPost("{contestId:guid}/finish")]
        public async Task<IActionResult> FinishContest([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var command = new PublishContestCommand(contestId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/contests/{contestId}/archive
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
        [HttpGet("{contestId:guid}/problems")]
        public async Task<IActionResult> GetContestProblems([FromRoute] Guid contestId, CancellationToken cancellationToken)
        {
            var query = new GetContestProblemsQuery(contestId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // POST api/contests/{contestId}/problems
        [HttpPost("{contestId:guid}/problems")]
        public async Task<IActionResult> AddProblemToContest([FromRoute] Guid contestId, [FromBody] AddProblemToContestRequest request, CancellationToken cancellationToken)
        {
            var command = new AddProblemToContestCommand(contestId, request.ProblemId, request.Code, request.Points);

            var problemId = await _mediator.Send(command, cancellationToken);

            return Ok(problemId);
        }

        // DELETE api/contests/{contestId}/problems/{problemId}
        [HttpDelete("{contestId:guid}/problems/{problemId:guid}")]
        public async Task<IActionResult> RemoveProblemFromContest([FromRoute] Guid contestId, [FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var command = new RemoveProblemFromContestCommand(contestId, problemId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // UPDATE api/contests/{contestId}/problems/order
        [HttpPut("{contestId:guid}/problems/order")]
        public async Task<IActionResult> ReorderContestProblems([FromRoute] Guid contestId, [FromBody] ReorderContestProblemsRequest request, CancellationToken cancellationToken)
        {
            var command = new ReorderContestProblemsCommand(contestId, request.ProblemIds);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // UPDATE api/contests/{contestId}/problems/{problemId}/points
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
        [HttpPost("{contestId:guid}/participants")]
        public async Task<IActionResult> JoinContest([FromRoute] Guid contestId, [FromBody] JoinContestRequest request, CancellationToken cancellationToken)
        {
            var command = new JoinContestCommand(contestId, request.UserId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/contests/{contestId}/participants/{userId}
        [HttpDelete("{contestId:guid}/participants/{userId:guid}")]
        public async Task<IActionResult> LeaveContest([FromRoute] Guid contestId, [FromRoute] Guid userId, CancellationToken cancellationToken)
        {
            var command = new LeaveContestCommand(contestId, userId);

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
    }
}