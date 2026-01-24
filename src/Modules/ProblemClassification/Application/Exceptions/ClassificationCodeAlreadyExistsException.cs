using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemClassification.Application.Exceptions
{
    public sealed class ClassificationCodeAlreadyExistsException : DomainException
    {
        public ClassificationCodeAlreadyExistsException(string code)
            : base($"Classification code '{code}' already exists.") { }
    }
}