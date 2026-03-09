using MediatR;
using Microsoft.AspNetCore.Mvc;
using VAlgo.API.Controllers.Discussions.Requests;
using VAlgo.Modules.Discussions.Application.Commands.AddComment;
using VAlgo.Modules.Discussions.Application.Commands.CreateDiscussion;
using VAlgo.Modules.Discussions.Application.Commands.DeleteComment;
using VAlgo.Modules.Discussions.Application.Commands.DeleteDiscussion;
using VAlgo.Modules.Discussions.Application.Commands.LockDiscussion;
using VAlgo.Modules.Discussions.Application.Commands.UnLockDiscussion;
using VAlgo.Modules.Discussions.Application.Commands.UpdateComment;
using VAlgo.Modules.Discussions.Application.Commands.UpdateDiscussion;
using VAlgo.Modules.Discussions.Application.Queries.GetDiscussionComments;
using VAlgo.Modules.Discussions.Application.Queries.GetDiscussionDetail;
using VAlgo.Modules.Discussions.Application.Queries.GetProblemDiscussions;

namespace VAlgo.API.Controllers.Discussions
{
    [ApiController]
    [Route("api")]
    public sealed class DiscussionsController : Controller
    {
        private readonly IMediator _mediator;

        public DiscussionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Lifecycle Discussions
        #endregion

        // POST api/problems/{problemId}/discussions
        [HttpPost("problems/{problemId:guid}/discussions")]
        public async Task<IActionResult> CreateDiscussion([FromRoute] Guid problemId, [FromBody] CreateDiscussionRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateDiscussionCommand(
                problemId,
                request.AuthorId,
                request.Title,
                request.Content
            );

            var discussionId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetDiscussionDetail), new { discussionId }, new { id = discussionId });
        }

        // PUT api/discussions/{discussionId}
        [HttpPut("discussions/{discussionId:guid}")]
        public async Task<IActionResult> UpdateDiscussion([FromRoute] Guid discussionId, [FromBody] UpdateDiscussionRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateDiscussionCommand(
                discussionId,
                request.UserId,
                request.Title,
                request.Content
            );

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/discussions/{discussionId}
        [HttpDelete("discussions/{discussionId:guid}")]
        public async Task<IActionResult> DeleteDiscussion([FromRoute] Guid discussionId, [FromBody] DeleteDiscussionRequest request, CancellationToken cancellationToken)
        {
            var command = new DeleteDiscussionCommand(discussionId, request.UserId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/discussions/{discussionId}/lock
        [HttpPost("discussions/{discussionId:guid}/lock")]
        public async Task<IActionResult> LockDiscussion([FromRoute] Guid discussionId, [FromBody] LockDiscussionRequest request, CancellationToken cancellationToken)
        {
            var command = new LockDiscussionCommand(discussionId, request.UserId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // POST api/discussions/{discussionId}/unlock
        [HttpPost("discussions/{discussionId:guid}/unlock")]
        public async Task<IActionResult> UnLockDiscussion([FromRoute] Guid discussionId, [FromBody] UnlockDiscussionRequest request, CancellationToken cancellationToken)
        {
            var command = new UnLockDiscussionCommand(discussionId, request.UserId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        #region Lifecycle Comment
        #endregion

        // POST api/discussions/{discussionId}/comments
        [HttpPost("discussions/{discussionId:guid}/comments")]
        public async Task<IActionResult> AddComment([FromRoute] Guid discussionId, [FromBody] AddCommentRequest request, CancellationToken cancellationToken)
        {
            var command = new AddCommentCommand(
                discussionId,
                request.AuthorId,
                request.Content,
                request.ParentCommentId
            );

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }


        // PUT api/discussions/{discussionId}/comments/{commentId}
        [HttpPut("discussions/{discussionId:guid}/comments/{commentId:guid}")]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid discussionId, [FromRoute] Guid commentId, [FromBody] UpdateCommentRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateCommentCommand(
                discussionId,
                commentId,
                request.UserId,
                request.Content
            );

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        // DELETE api/discussions/{discussionId}/comments/{commentId}
        [HttpDelete("discussions/{discussionId:guid}/comments/{commentId:guid}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid discussionId, [FromRoute] Guid commentId, [FromBody] DeleteCommentRequest request, CancellationToken cancellationToken)
        {
            var command = new DeleteCommentCommand(
                discussionId,
                commentId,
                request.UserId
            );

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        #region Queries
        #endregion

        // GET api/discussions/{discussionId}
        [HttpGet("discussions/{discussionId:guid}")]
        public async Task<IActionResult> GetDiscussionDetail([FromRoute] Guid discussionId, CancellationToken cancellationToken)
        {
            var query = new GetDiscussionDetailQuery(discussionId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/problems/{problemId}/discussions
        [HttpGet("problems/{problemId:guid}/discussions")]
        public async Task<IActionResult> GetProblemDiscussions([FromRoute] Guid problemId, [FromQuery] int page, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            var query = new GetProblemDiscussionsQuery(problemId, page, pageSize);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET api/discussions/{discussionId}/comments
        [HttpGet("discussions/{discussionId:guid}/comments")]
        public async Task<IActionResult> GetDiscussionComments([FromRoute] Guid discussionId, [FromQuery] int page, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            var query = new GetDiscussionCommentsQuery(discussionId, page, pageSize);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}