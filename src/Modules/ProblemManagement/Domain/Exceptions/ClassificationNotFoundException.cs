using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class ClassificationNotFoundException : DomainException
    {
        public ClassificationNotFoundException(Guid value) : base($"Classification this is {value} not found.") { }
    }
}