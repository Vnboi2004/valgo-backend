using MediatR;

namespace VAlgo.Modules.Submissions.Application.Commands.CreateSubmission
{
    public sealed record CreateSubmissionCommand(
        Guid UserId,
        Guid ProblemId,
        string Language,
        string SourceCode
    ) : IRequest<Guid>;
}