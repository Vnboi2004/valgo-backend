using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class CodeTemplateNotFoundException : DomainException
    {
        public CodeTemplateNotFoundException(string language) : base($"Code template for language '{language}' was not founds.") { }
    }
}