using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class AllowedLanguageNotFoundException : DomainException
    {
        public AllowedLanguageNotFoundException(string language) : base($"Allowed language '{language}' not found.") { }
    }
}