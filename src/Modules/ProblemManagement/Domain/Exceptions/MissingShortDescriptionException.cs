using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class MissingShortDescriptionException : DomainException
    {
        public MissingShortDescriptionException(Guid problemId) : base($"Problem '{problemId}' missing short description.") { }
    }
}