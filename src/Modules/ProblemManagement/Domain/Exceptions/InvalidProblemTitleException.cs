using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidProblemTitleException : DomainException
    {
        public InvalidProblemTitleException() : base("Title not empty") { }
    }
}