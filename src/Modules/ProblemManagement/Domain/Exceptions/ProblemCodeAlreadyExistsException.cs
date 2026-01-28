using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class ProblemCodeAlreadyExistsException : DomainException
    {
        public ProblemCodeAlreadyExistsException(string reason) : base(reason) { }
    }
}