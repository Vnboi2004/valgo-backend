using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.ProblemManagement.Requests;
using VAlgo.Modules.ProblemManagement.Application.Commands.AddAllowedLanguage;
using VAlgo.Modules.ProblemManagement.Application.Commands.AddCompany;
using VAlgo.Modules.ProblemManagement.Application.Commands.AddExample;
using VAlgo.Modules.ProblemManagement.Application.Commands.AddHint;
using VAlgo.Modules.ProblemManagement.Application.Commands.AddSimilarProblem;
using VAlgo.Modules.ProblemManagement.Application.Commands.AddTestCase;
using VAlgo.Modules.ProblemManagement.Application.Commands.ArchiveProblem;
using VAlgo.Modules.ProblemManagement.Application.Commands.AssignClassification;
using VAlgo.Modules.ProblemManagement.Application.Commands.CreateProblem;
using VAlgo.Modules.ProblemManagement.Application.Commands.DeleteCompany;
using VAlgo.Modules.ProblemManagement.Application.Commands.DeleteExample;
using VAlgo.Modules.ProblemManagement.Application.Commands.DeleteHint;
using VAlgo.Modules.ProblemManagement.Application.Commands.DeleteSimilarProblem;
using VAlgo.Modules.ProblemManagement.Application.Commands.PublishProblem;
using VAlgo.Modules.ProblemManagement.Application.Commands.RemoveAllowedLanguage;
using VAlgo.Modules.ProblemManagement.Application.Commands.RemoveTestCase;
using VAlgo.Modules.ProblemManagement.Application.Commands.ReorderExamples;
using VAlgo.Modules.ProblemManagement.Application.Commands.ReorderHints;
using VAlgo.Modules.ProblemManagement.Application.Commands.ReorderTestCases;
using VAlgo.Modules.ProblemManagement.Application.Commands.UnassignClassification;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateConstraints;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateExample;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateHint;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemContent;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemDifficulty;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemEditorial;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemMetadata;
using VAlgo.Modules.ProblemManagement.Application.Commands.UpdateTestCase;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemCompanies;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemForJudge;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemStats;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemTags;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetSimilarProblems;
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

        // PUT /problems/{problemId}/content
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/content")]
        public async Task<IActionResult> UpdateProblemContent([FromRoute] Guid problemId, [FromBody] UpdateProblemContentRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProblemContentCommand(
                problemId,
                request.Statement,
                request.Constraints,
                request.InputFormat,
                request.OutputFormat,
                request.FollowUp
            );

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // Trên LeetCode tương đương với tab **"Solution"** — nơi admin/problem setter đăng solution chính thức, giải thích approach, complexity analysis sau khi bài đã public.
        // PUT /problems/{problemId}/editorial
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/editorial")]
        public async Task<IActionResult> UpdateProblemEditorial([FromRoute] Guid problemId, [FromBody] UpdateProblemEditorialRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProblemEditorialCommand(problemId, request.Editorial);

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

        // PUT api/problems/{id}/testcases/{testCaseId}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/testcases/{testCaseId:guid}")]
        public async Task<IActionResult> UpdateTestCase([FromRoute] Guid problemId, [FromRoute] Guid testCaseId, [FromBody] UpdateTestCaseRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTestCaseCommand(
                problemId,
                testCaseId,
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

        // PUT api/problems/{id}/testcases/reorder
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/testcases/reorder")]
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
        [HttpDelete("{problemId:guid}/languages/{language}")]
        public async Task<IActionResult> RemoveAllowedLanguage([FromRoute] Guid problemId, [FromRoute] string language, CancellationToken cancellationToken)
        {
            var command = new RemoveAllowedLanguageCommand(problemId, language);

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


        // use-case: Example
        // POST api/problems/{problemId}/examples
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPost("{problemId:guid}/examples")]
        public async Task<IActionResult> AddExample([FromRoute] Guid problemId, [FromBody] AddExampleRequest request, CancellationToken cancellationToken)
        {
            var command = new AddExampleCommand(problemId, request.Input, request.Output, request.Explanation);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/problems/{problemId}/examples/{exampleId}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/examples/{exampleId:guid}")]
        public async Task<IActionResult> UpdateExample([FromRoute] Guid problemId, [FromRoute] Guid exampleId, [FromBody] UpdateExampleRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateExampleCommand(problemId, exampleId, request.Input, request.Output, request.Explanation);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/problems/{problemId}/examples/{exampleId}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpDelete("{problemId:guid}/examples/{exampleId:guid}")]
        public async Task<IActionResult> DeleteExample([FromRoute] Guid problemId, [FromRoute] Guid exampleId, CancellationToken cancellationToken)
        {
            var command = new DeleteExampleCommand(problemId, exampleId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/problems/{problemId}/examples/reorder
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/examples/reorder")]
        public async Task<IActionResult> ReorderExamples([FromRoute] Guid problemId, [FromBody] ReorderExamplesRequest request, CancellationToken cancellationToken)
        {
            var command = new ReorderExamplesCommand(problemId, request.ExampleIds);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // use-case: hints
        // POST api/problems/{problemId}/hints
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPost("{problemId:guid}/hints")]
        public async Task<IActionResult> AddHint([FromRoute] Guid problemId, [FromBody] AddHintRequest request, CancellationToken cancellationToken)
        {
            var command = new AddHintCommand(problemId, request.Content);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/problems/{problemId}/hints/{hintId}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/hints/{hintId:guid}")]
        public async Task<IActionResult> UpdateHint([FromRoute] Guid problemId, [FromRoute] Guid hintId, [FromBody] UpdateHintRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateHintCommand(problemId, hintId, request.Content);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/problems/{problemId}/hints/{hintId}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpDelete("{problemId:guid}/hints/{hintId:guid}")]
        public async Task<IActionResult> DeleteHint([FromRoute] Guid problemId, [FromRoute] Guid hintId, CancellationToken cancellationToken)
        {
            var command = new DeleteHintCommand(problemId, hintId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // PUT api/problems/{problemId}/hints/reorder
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPut("{problemId:guid}/hints/reorder")]
        public async Task<IActionResult> ReorderHints([FromRoute] Guid problemId, [FromBody] ReorderHintsRequest request, CancellationToken cancellationToken)
        {
            var command = new ReorderHintsCommand(problemId, request.HintIds);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // use-case: companies
        // POST api/problems/{problemId}/companies
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPost("{problemId:guid}/companies")]
        public async Task<IActionResult> AssignCompany([FromRoute] Guid problemId, [FromBody] AssignCompanyRequest request, CancellationToken cancellationToken)
        {
            var command = new AddCompanyCommand(problemId, request.CompanyId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/problems/{problemId}/companies/{companyId}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpDelete("{problemId:guid}/companies/{companyId:guid}")]
        public async Task<IActionResult> UnassignCompany([FromRoute] Guid problemId, [FromRoute] Guid companyId, CancellationToken cancellationToken)
        {
            var command = new DeleteCompanyCommand(problemId, companyId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // use-case: similar
        // POST api/problems/{problemId}/similars
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpPost("{problemId:guid}/similars")]
        public async Task<IActionResult> AddSimilarProblem([FromRoute] Guid problemId, [FromBody] AddSimilarProblemRequest request, CancellationToken cancellationToken)
        {
            var command = new AddSimilarProblemCommand(problemId, request.SimilarProblemId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/problems/{problemId}/similar/{similarProblemId}
        [Authorize(Roles = "Admin,ProblemSetter")]
        [HttpDelete("{problemId:guid}/similars/{similarId:guid}")]
        public async Task<IActionResult> DeleteSimilarProblem([FromRoute] Guid problemId, [FromRoute] Guid similarId, CancellationToken cancellationToken)
        {
            var command = new DeleteSimilarProblemCommand(problemId, similarId);

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
            [FromQuery] Guid? companyId,
            [FromQuery] Guid? classificationId,
            [FromQuery] ProblemSortBy sortBy,
            [FromQuery] SortDirection sortDirection,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default
        )
        {
            var query = new GetProblemListQuery(
                keyword,
                difficulty,
                status,
                companyId,
                classificationId,
                sortBy,
                sortDirection,
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

        // GET api/problems/{problemId}/similar
        [AllowAnonymous]
        [HttpGet("{problemId:guid}/similar")]
        public async Task<IActionResult> GetSimilarProblems([FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var query = new GetSimilarProblemsQuery(problemId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/problems/{problemId}/companies
        [AllowAnonymous]
        [HttpGet("{problemId:guid}/companies")]
        public async Task<IActionResult> GetProblemCompanies([FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var query = new GetProblemCompaniesQuery(problemId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/problems/{problemId}/tags
        [AllowAnonymous]
        [HttpGet("{problemId:guid}/tags")]
        public async Task<IActionResult> GetProblemTags([FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var query = new GetProblemTagsQuery(problemId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/problems/{problemId}/stats
        [AllowAnonymous]
        [HttpGet("{problemId:guid}/stats")]
        public async Task<IActionResult> GetProblemStats([FromRoute] Guid problemId, CancellationToken cancellationToken)
        {
            var query = new GetProblemStatsQuery(problemId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

    }
}