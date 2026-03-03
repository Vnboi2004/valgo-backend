using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidTimeLimitException : DomainException
    {
        public InvalidTimeLimitException(int value) : base($"Invalid time limit: {value} ms.") { }
    }
}