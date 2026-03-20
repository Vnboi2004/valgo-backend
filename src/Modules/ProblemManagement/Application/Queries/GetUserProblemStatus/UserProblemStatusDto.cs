using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetUserProblemStatus
{
    public sealed class UserProblemStatusDto
    {
        public Guid ProblemId { get; init; }
        public UserProblemStatus? Status { get; init; }
    }
}