using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemClassification.Application.Exceptions
{
    public sealed class ClassificationNotFoundException : DomainException
    {
        public ClassificationNotFoundException(ClassificationId classificationId)
            : base($"Classification with id '{classificationId.Value}' was not found.") { }
    }
}