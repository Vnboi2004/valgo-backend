using MediatR;

namespace VAlgo.Modules.Submissions.Application.Commands.CreateSubmission
{
    public sealed record CreateSubmissionCommand(
        Guid ProblemId,
        Guid? ContestId,
        string Language,
        string SourceCode
    ) : IRequest<Guid>;
}