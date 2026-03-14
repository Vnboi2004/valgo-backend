using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.ProblemManagement.Requests;
using VAlgo.Modules.ProblemManagement.Application.Commands.AddAllowedLanguage;
using VAlgo.Modules.ProblemManagement.Application.Commands.AddTestCase;
using VAlgo.Modules.ProblemManagement.Application.Commands.ArchiveProblem;
using VAlgo.Modules.ProblemManagement.Application.Commands.AssignClassification;
using VAlgo.Modules.ProblemManagement.Application.Commands.CreateProblem;
using VAlgo.Modules.ProblemManagement.Application.Commands.PublishProblem;
using VAlgo.Modules.ProblemManagement.Application.Commands.RemoveAllowedLanguage;
using VAlgo.Modules.ProblemManagement.Application.Commands.RemoveTestCase;
using VAlgo.Modules.ProblemManagement.Application.Commands.ReorderTestCases;
using VAlgo.Modules.ProblemManagement.Application.Commands.UnassignClassification;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateConstraints;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemDifficulty;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemMetadata;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemForJudge;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList;
using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.API.Controllers.ProblemManagement
{
    [ApiController]
    [Route("api/problems")]
    [Authorize]
    public sealed class ProblemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProblemsController(IMediator mediator) => _mediator = mediator;

        // use-case: Problem lifecycle
        // POST api/problems/
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPost]
        public async Task<IActionResult> CreateProblem([FromBody] CreateProblemRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateProblemCommand(
                request.Code,
                request.Title,
                request.Statement,
                request.ShortDescription,
                request.Difficulty,
                request.TimeLimitMs,
                request.MemoryLimitKb
            );

            var problemId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetProblemDetail), new { problemId }, new { ProblemId = problemId });
        }

        // POST api/problems/{id}/publish
        [Authorize(Roles = "Admin")]
        [HttpPost("{problemId:guid}/publish")]
        public async Task<IActionResult> PublishProblem([FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var command = new PublishProblemCommand(problemId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/problems/{id}/archive
        [Authorize(Roles = "Admin")]
        [HttpPost("{problemId:guid}/archive")]
        public async Task<IActionResult> ArchiveProblem([FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var command = new ArchiveProblemCommand(problemId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/problems/{id}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}")]
        public async Task<IActionResult> UpdateProblemMetadata([FromRoute] Guid problemId, [FromBody] UpdateProblemMetadataRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProblemMetadataCommand(
                problemId,
                request.Title,
                request.Statement,
                request.ShortDescription
            );

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/problems/{id}/constraints
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/constraints")]
        public async Task<IActionResult> UpdateConstraints([FromRoute] Guid problemId, [FromBody] UpdateConstraintsRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateConstraintsCommand(
                problemId,
                request.TimeLimitMs,
                request.MemoryLimitKb
            );

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/problems/{id}/difficulty
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/difficulty")]
        public async Task<IActionResult> UpdateProblemDifficulty([FromRoute] Guid problemId, [FromBody] UpdateProblemDifficultyRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProblemDifficultyCommand(problemId, request.Difficulty);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // use-case: TestCase lificycle
        // POST api/problems/{id}/testcases
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPost("{problemId:guid}/testcases")]
        public async Task<IActionResult> AddTestCase([FromRoute] Guid problemId, [FromBody] AddTestCaseRequest request, CancellationToken cancellationToken)
        {
            var command = new AddTestCaseCommand(
                problemId,
                request.Input,
                request.ExpectedOutput,
                request.OutputComparisonStrategy,
                request.IsSample
            );

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/problems/{id}/testcases/{testCaseId}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpDelete("{problemId:guid}/testcases/{testCaseId:guid}")]
        public async Task<IActionResult> RemoveTestCase([FromRoute] Guid problemId, [FromRoute] Guid testCaseId, CancellationToken cancellationToken)
        {
            var command = new RemoveTestCaseCommand(problemId, testCaseId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/problems/{id}/testcases/order
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/testcases/order")]
        public async Task<IActionResult> ReorderTestCases([FromRoute] Guid problemId, [FromBody] ReorderTestCasesRequest request, CancellationToken cancellationToken)
        {
            var comnmand = new ReorderTestCasesCommand(problemId, request.OrderedTestCaseIds);

            await _mediator.Send(comnmand, cancellationToken);

            return NoContent();
        }

        // use-case: AllowedLanguage
        // POST api/problems/{id}/languages
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPost("{problemId:guid}/languages")]
        public async Task<IActionResult> AddAllowedLanguage([FromRoute] Guid problemId, [FromBody] AddAllowedLanguageRequest request, CancellationToken cancellationToken)
        {
            var command = new AddAllowedLanguageCommand(problemId, request.Language);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/problems/{id}/languages/{language}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpDelete("{problemId:guid}/languages")]
        public async Task<IActionResult> RemoveAllowedLanguage([FromRoute] Guid problemId, [FromBody] RemoveAllowedLanguageRequest request, CancellationToken cancellationToken)
        {
            var command = new RemoveAllowedLanguageCommand(problemId, request.Language);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // use-case: Classification
        // POST api/problems/{id}/classification
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPost("{problemId:guid}/classification")]
        public async Task<IActionResult> AssignClassification([FromRoute] Guid problemId, [FromBody] AssignClassificationRequest request, CancellationToken cancellationToken)
        {
            var command = new AssignClassificationCommand(problemId, request.ClassificationId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/problems/{id}/classifications/{classificationId}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpDelete("{problemId:guid}/classifications/{classificationId:guid}")]
        public async Task<IActionResult> UnassignClassification([FromRoute] Guid problemId, [FromRoute] Guid classificationId, CancellationToken cancellationToken)
        {
            var command = new UnassignClassificationCommand(problemId, classificationId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // Query side
        // GET api/problems/{id}
        [AllowAnonymous]
        [HttpGet("{problemId:guid}")]
        public async Task<IActionResult> GetProblemDetail([FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var query = new GetProblemDetailQuery(problemId);

            var problem = await _mediator.Send(query, cancellationToken);

            return Ok(problem);
        }

        // GET api/problems
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProblemList(
            [FromQuery] string? keyword,
            [FromQuery] Difficulty? difficulty,
            [FromQuery] ProblemStatus? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default
        )
        {
            var query = new GetProblemListQuery(
                keyword,
                difficulty,
                status,
                page,
                pageSize
            );

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/problems/{id}/editor
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpGet("{problemId:guid}/editor")]
        public async Task<IActionResult> GetProblemEditor([FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var query = new GetProblemEditorQuery(problemId);

            var problem = await _mediator.Send(query, cancellationToken);

            return Ok(problem);
        }

        // GET api/problems/{id}/judge
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpGet("{problemId:guid}/judge")]
        public async Task<IActionResult> GetProblemForJudge([FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var query = new GetProblemForJudgeQuery(problemId);

            var problem = await _mediator.Send(query, cancellationToken);

            return Ok(problem);
        }
    }
}