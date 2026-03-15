using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidProblemHintException : DomainException
    {
        public InvalidProblemHintException() : base("Hint invalid") { }
    }
}