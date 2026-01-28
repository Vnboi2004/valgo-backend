using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemClassification.Domain.Exceptions
{
    public sealed class ClassificationCodeInvalidException : DomainException
    {
        public ClassificationCodeInvalidException() : base("Classification code must not be empty.") { }
    }
}