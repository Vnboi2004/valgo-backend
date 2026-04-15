
using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly SubmissionsDbContext _dbContext;
        private readonly IMediator _mediator;

        public UnitOfWork(SubmissionsDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            await DispatchDomainEventsAsync();

            return result;
        }

        public async Task DispatchDomainEventsAsync()
        {
            var entities = _dbContext.ChangeTracker
                    .Entries<IHasDomainEvents>()
                    .Where(x => x.Entity.DomainEvents.Any())
                    .ToList();

            var domainEvents = entities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            entities.ForEach(e => e.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}