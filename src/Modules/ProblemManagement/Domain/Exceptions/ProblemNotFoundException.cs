using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class ProblemNotFoundException : DomainException
    {
        public ProblemNotFoundException(Guid value) : base($"Problem is a {value} not found.") { }
    }
}