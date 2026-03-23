using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class DuplicateCodeTemplateException : DomainException
    {
        public DuplicateCodeTemplateException(string language) : base($"Code template for language '{language}' already exists.") { }
    }
}