using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemClassification.Domain.Exceptions
{
    public sealed class ClassificationInactiveException : DomainException
    {
        public ClassificationInactiveException() : base("Cannot modify an inactive classification.") { }
    }
}