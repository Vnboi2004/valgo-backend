using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidLanguageException : DomainException
    {
        public InvalidLanguageException() : base("Invalid programing language.") { }
    }
}