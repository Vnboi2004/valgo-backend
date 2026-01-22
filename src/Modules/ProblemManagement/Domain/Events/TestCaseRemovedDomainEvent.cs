using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Events
{
    public sealed record TestCaseRemovedDomainEvent(Guid ProblemId, Guid TestCaseId, DateTime OccurredOn) : IDomainEvent;
}