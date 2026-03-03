using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Events
{
    public sealed record ProblemPublishedDomainEvent(Guid ProblemId, DateTime OccurredOn) : IDomainEvent;
}