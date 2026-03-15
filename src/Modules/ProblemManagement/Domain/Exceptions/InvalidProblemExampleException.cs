using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidProblemExampleException : DomainException
    {
        public InvalidProblemExampleException() : base("Problem example invalid") { }
    }
}