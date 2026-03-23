using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using VAlgo.API.Controllers.Submissions.Requests;
using VAlgo.Modules.Submissions.Application.Commands.CancelSubmission;
using VAlgo.Modules.Submissions.Application.Commands.CompleteSubmission;
using VAlgo.Modules.Submissions.Application.Commands.CreateSubmission;
using VAlgo.Modules.Submissions.Application.Commands.EnqueueSubmission;
using VAlgo.Modules.Submissions.Application.Commands.FailSubmission;
using VAlgo.Modules.Submissions.Application.Commands.StartSubmissionExecution;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissions;

namespace VAlgo.API.Controllers.Submissions
{
    [ApiController]
    [Route("api/submissions")]
    public sealed class SubmissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubmissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/submissions
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateSubmissionCommand(
                request.ProblemId,
                request.ContestId,
                request.Language,
                request.SourceCode
            );

            var submissionId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetSubmissionDetail), new { submissionId }, new { SubmissionId = submissionId });
        }

        // POST api/submissions/{id}/start
        [Authorize(Roles = "User")]
        [HttpPost("{submissionId:guid}/start")]
        public async Task<IActionResult> StartSubmission([FromRoute] Guid submissionId, CancellationToken cancellationToken)
        {
            var command = new StartSubmissionExecutionCommand(submissionId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/submissions/{id}/complete
        [Authorize(Roles = "User")]
        [HttpPost("{submissionId:guid}/complete")]
        public async Task<IActionResult> CompleteSubmission([FromRoute] Guid submissionId, [FromBody] CompleteSubmissionRequest request, CancellationToken cancellationToken)
        {
            var command = new CompleteSubmissionCommand(
                submissionId,
                request.Verdict,
                request.PassedTestCases,
                request.TotalTestCases,
                request.TimeMs,
                request.MemoryKb,
                request.TestCases
            );

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/submissions/{id}/
        [Authorize(Roles = "User")]
        [HttpPost("{submissionId:guid}/fail")]
        public async Task<IActionResult> FailSubmission([FromRoute] Guid submissionId, [FromBody] FailSubmissionRequest request, CancellationToken cancellationToken)
        {
            var command = new FailSubmissionCommand(submissionId, request.Reason);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/submissions/{id}/cancel
        [Authorize(Roles = "User")]
        [HttpPost("{submissionId:guid}/cancel")]
        public async Task<IActionResult> CancelSubmission([FromRoute] Guid submissionId, CancellationToken cancellationToken)
        {
            var command = new CancelSubmissionCommand(submissionId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/submissions/{id}/enqueue
        [Authorize(Roles = "User")]
        [HttpPost("{submissionId:guid}/enqueue")]
        public async Task<IActionResult> EnqueueSumbission([FromRoute] Guid submissionId, CancellationToken cancellationToken)
        {
            var command = new EnqueueSubmissionCommand(submissionId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // GET api/submissions
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSubmissionList(
            [FromQuery] Guid? userId,
            [FromQuery] Guid? problemId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default
        )
        {
            var query = new GetSubmissionsQuery(userId, problemId, page, pageSize);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/submissions/{id}
        [Authorize]
        [HttpGet("{submissionId:guid}")]
        public async Task<IActionResult> GetSubmissionDetail([FromRoute] Guid submissionId, CancellationToken cancellationToken)
        {
            var query = new GetSubmissionDetailQuery(submissionId);

            var submission = await _mediator.Send(query, cancellationToken);

            return Ok(submission);
        }
    }
}