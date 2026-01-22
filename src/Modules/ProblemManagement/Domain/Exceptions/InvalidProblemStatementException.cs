using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidProblemStatementException : DomainException
    {
        public InvalidProblemStatementException() : base("Title not empty") { }
    }
}