using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidMemoryLimitException : DomainException
    {
        public InvalidMemoryLimitException(int value) : base($"Invalid memory limit: {value} KB.") { }
    }
}