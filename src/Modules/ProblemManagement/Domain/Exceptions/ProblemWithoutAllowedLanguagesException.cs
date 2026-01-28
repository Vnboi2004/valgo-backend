using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class ProblemWithoutAllowedLanguagesException : DomainException
    {
        public ProblemWithoutAllowedLanguagesException(Guid problemId) : base($"Problem '{problemId}' has no allowed language programing.") { }
    }
}