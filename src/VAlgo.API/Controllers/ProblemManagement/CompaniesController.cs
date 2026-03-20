using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.ProblemManagement.Requests;
using VAlgo.Modules.ProblemManagement.Application.Commands.CreateCompany;
using VAlgo.Modules.ProblemManagement.Application.Commands.DeactivateCompany;
using VAlgo.Modules.ProblemManagement.Application.Commands.ReactivateCompany;
using VAlgo.Modules.ProblemManagement.Application.Commands.RenameCompany;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetActiveCompanies;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyList;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyStats;

namespace VAlgo.API.Controllers.ProblemManagement
{
    [ApiController]
    [Route("api/companies")]
    public sealed class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/companies
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateCompanyCommand(request.Name);

            var companyId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetCompanyDetail), new { companyId }, new { CompanyId = companyId });
        }

        // PUT api/companies/{companyId}
        [Authorize(Roles = "Admin")]
        [HttpPut("{companyId:guid}")]
        public async Task<IActionResult> RenameCompany([FromRoute] Guid companyId, [FromBody] RenameCompanyRequest request, CancellationToken cancellationToken = default)
        {
            var command = new RenameCompanyCommand(companyId, request.Name);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/companies/{companyId}/deactivate
        [Authorize(Roles = "Admin")]
        [HttpPost("{companyId:guid}/deactivate")]
        public async Task<IActionResult> DeactivateCompany([FromRoute] Guid companyId, CancellationToken cancellationToken = default)
        {
            var command = new DeactivateCompanyCommand(companyId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/companies/{companyId}/reactivate
        [Authorize(Roles = "Admin")]
        [HttpPost("{companyId:guid}/reactivate")]
        public async Task<IActionResult> ReactivateCompany([FromRoute] Guid companyId, CancellationToken cancellationToken = default)
        {
            var command = new ReactivateCompanyCommand(companyId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // GET api/companies
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCompanyList([FromQuery] bool? isActive, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetCompanyListQuery(isActive, page, pageSize);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/companies/{companyId}
        [AllowAnonymous]
        [HttpGet("{companyId:guid}")]
        public async Task<IActionResult> GetCompanyDetail([FromRoute] Guid companyId, CancellationToken cancellationToken = default)
        {
            var query = new GetCompanyDetailQuery(companyId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/companies/active
        [AllowAnonymous]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCompanies([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetActiveCompaniesQuery(page, pageSize);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/companies/stats
        [AllowAnonymous]
        [HttpGet("stats")]
        public async Task<IActionResult> GetCompanyStats(CancellationToken cancellationToken)
        {
            var query = new GetCompanyStatsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}