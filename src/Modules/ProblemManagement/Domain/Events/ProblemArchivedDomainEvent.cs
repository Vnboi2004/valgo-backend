using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed record ProblemArchivedDomainEvent(Guid ProblemId, DateTime OccurredOn) : IDomainEvent;
}