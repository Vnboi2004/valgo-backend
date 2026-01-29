using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VAlgo.API.Controllers.Identity
{
    [ApiController]
    [Route("api/identity/admin/users")]
    [Authorize(Roles = "Admin")]
    public sealed class AdminUsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }


    }
}