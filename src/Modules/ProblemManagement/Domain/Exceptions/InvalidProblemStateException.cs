using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidProblemStateException : DomainException
    {
        public InvalidProblemStateException(Guid problemId, ProblemStatus status)
            : base($"Problem '{problemId}' is in invalid state: {status}.") { }
    }
}