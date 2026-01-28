using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Events
{
    public sealed record AllowedLanguageRemovedDomainEvent(Guid ProblemId, string Language, DateTime OccurredOn) : IDomainEvent;
}