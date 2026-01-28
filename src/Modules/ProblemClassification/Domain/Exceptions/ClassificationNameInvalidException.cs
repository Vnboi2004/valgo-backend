using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemClassification.Domain.Exceptions
{
    public sealed class ClassificationNameInvalidException : DomainException
    {
        public ClassificationNameInvalidException() : base("Classification name must not be empty.") { }
    }
}