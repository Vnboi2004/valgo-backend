using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidSimilarProblemException : DomainException
    {
        public InvalidSimilarProblemException() : base("Similar invalid") { }
    }
}