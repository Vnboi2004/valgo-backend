using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Submissions.Domain.Exceptions
{
    public sealed class InvalidLanguageException : DomainException
    {
        public InvalidLanguageException() : base("Language is requried") { }
    }
}