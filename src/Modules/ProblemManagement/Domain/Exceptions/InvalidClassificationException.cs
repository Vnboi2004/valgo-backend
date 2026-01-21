using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidClassificationException : DomainException
    {
        public InvalidClassificationException() : base("Classification is inactive") { }
    }
}