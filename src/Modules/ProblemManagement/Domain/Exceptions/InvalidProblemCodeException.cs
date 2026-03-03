using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidProblemCodeException : DomainException
    {
        public InvalidProblemCodeException() : base("Code is required.") { }
    }
}