using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class ProblemAlreadyPublishedException : DomainException
    {
        public ProblemAlreadyPublishedException(Guid problemId) : base($"Problem '{problemId}' is not in Draft state.") { }
    }
}