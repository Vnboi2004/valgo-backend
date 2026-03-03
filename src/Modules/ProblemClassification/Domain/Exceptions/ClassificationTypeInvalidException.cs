using VAlgo.Modules.ProblemClassification.Domain.Enums;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemClassification.Domain.Exceptions
{
    public sealed class ClassificationTypeInvalidException : DomainException
    {
        public ClassificationTypeInvalidException(ClassificationType type) : base($"Invalid classification type: {type}.") { }
    }
}