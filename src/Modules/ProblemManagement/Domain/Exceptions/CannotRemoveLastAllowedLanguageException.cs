using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class CannotRemoveLastAllowedLanguageException : DomainException
    {
        public CannotRemoveLastAllowedLanguageException(Guid problemId)
            : base($"Problem '{problemId}' must have at least one allowed language.") { }
    }
}