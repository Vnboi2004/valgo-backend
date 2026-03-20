using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.ProblemClassification.Requests;
using VAlgo.Modules.ProblemClassification.Application.Commands.CreateClassification;
using VAlgo.Modules.ProblemClassification.Application.Commands.DeactivateClassification;
using VAlgo.Modules.ProblemClassification.Application.Commands.ReactivateClassification;
using VAlgo.Modules.ProblemClassification.Application.Commands.RenameClassification;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetActiveClassifications;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetClassificationDetail;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetClassifications;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetClassificationStats;
using VAlgo.SharedKernel.CrossModule.Classifications;

namespace VAlgo.API.Controllers.ProblemClassification
{
    [ApiController]
    [Route("api/problem-classifications")]
    public sealed class ClassificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/problem-classifications
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateClassification([FromBody] CreateClassificationRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateClassificationCommand(
                request.Code,
                request.Name,
                request.Type
            );

            var classificationId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetClassificationDetail), new { classificationId }, new { ClassificationId = classificationId });
        }

        // PUT api/problem-classifications/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{classificationId:guid}")]
        public async Task<IActionResult> RenameClassification([FromRoute] Guid classificationId, [FromBody] RenameClassificationRequest request, CancellationToken cancellationToken)
        {
            var command = new RenameClassificationCommand(classificationId, request.NewName);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }


        // POST api/problem-classifications/{id}/deactivate
        [Authorize(Roles = "Admin")]
        [HttpPost("{classificationId:guid}/deactivate")]
        public async Task<IActionResult> DeactivateClassification([FromRoute] Guid classificationId, CancellationToken cancellationToken)
        {
            var command = new DeactivateClassificationCommand(classificationId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/problem-classifications/{id}/reactivate
        [Authorize(Roles = "Admin")]
        [HttpPost("{classificationId:guid}/reactivate")]
        public async Task<IActionResult> ReactivateClassification([FromRoute] Guid classificationId, CancellationToken cancellationToken)
        {
            var command = new ReactivateClassificationCommand(classificationId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // GET api/problem-classifications
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetClassificationList(
            [FromQuery] ClassificationType? type,
            [FromQuery] bool? isActive,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            CancellationToken cancellationToken
        )
        {
            var query = new GetClassificationsQuery(type, isActive, page, pageSize);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/problem-classifications/{id}
        [AllowAnonymous]
        [HttpGet("{classificationId:guid}")]
        public async Task<IActionResult> GetClassificationDetail([FromRoute] Guid classificationId, CancellationToken cancellationToken)
        {
            var query = new GetClassificationDetailQuery(classificationId);

            var classification = await _mediator.Send(query, cancellationToken);

            return Ok(classification);
        }

        // GET api/problem-classifications/active
        [AllowAnonymous]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveClassifications(
            [FromQuery] ClassificationType? type,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            CancellationToken cancellationToken
        )
        {
            var query = new GetActiveClassificationsQuery(type, page, pageSize);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/problem-classification/stats
        [AllowAnonymous]
        [HttpGet("stats")]
        public async Task<IActionResult> GetClassificationStats([FromQuery] ClassificationType? type, CancellationToken cancellationToken = default)
        {
            var query = new GetClassificationStatsQuery(type);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}