using MediatR;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.ProblemClassification.Requests;
using VAlgo.Modules.ProblemClassification.Application.Commands.CreateClassification;
using VAlgo.Modules.ProblemClassification.Application.Commands.DeactivateClassification;
using VAlgo.Modules.ProblemClassification.Application.Commands.ReactivateClassification;
using VAlgo.Modules.ProblemClassification.Application.Commands.RenameClassification;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetActiveClassifications;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetClassificationDetail;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetClassifications;
using VAlgo.Modules.ProblemClassification.Domain.Enums;

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
        [HttpPut("{classificationId:guid}")]
        public async Task<IActionResult> RenameClassification([FromRoute] Guid classificationId, [FromBody] RenameClassificationRequest request, CancellationToken cancellationToken)
        {
            var command = new RenameClassificationCommand(classificationId, request.NewName);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }


        // POST api/problem-classifications/{id}/deactivate
        [HttpPost("{classificationId:guid}/deactivate")]
        public async Task<IActionResult> DeactivateClassification([FromRoute] Guid classificationId, CancellationToken cancellationToken)
        {
            var command = new DeactivateClassificationCommand(classificationId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/problem-classifications/{id}/reactivate
        [HttpPost("{classificationId:guid}/reactivate")]
        public async Task<IActionResult> ReactivateClassification([FromRoute] Guid classificationId, CancellationToken cancellationToken)
        {
            var command = new ReactivateClassificationCommand(classificationId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // GET api/problem-classifications
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
        [HttpGet("{classificationId:guid}")]
        public async Task<IActionResult> GetClassificationDetail([FromRoute] Guid classificationId, CancellationToken cancellationToken)
        {
            var query = new GetClassificationDetailQuery(classificationId);

            var classification = await _mediator.Send(query, cancellationToken);

            return Ok(classification);
        }

        // GET api/problem-classifications/active
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
    }
}