using AsanPardakht.Core.Data;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Core.Domain;
using AsanPardakht.Core.Domain.Events;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.UnitOfWork
{
    public sealed class EfCoreUnitOfWork : IUnitOfWork
    {
        private readonly IDispatcher _dispatcher;
        private readonly ApplicationDbContext _applicationDbContext;

        public EfCoreUnitOfWork(ApplicationDbContext applicationDbContext, IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _applicationDbContext = applicationDbContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            List<IDomainEvent> domainEvents = _applicationDbContext.ChangeTracker.Entries<IAggregateRoot>().SelectMany(x => x.Entity.DomainEvents).ToList();

            if (domainEvents != null && domainEvents.Count > 0)
            {
                foreach (IDomainEvent @event in domainEvents)
                {
                    await _dispatcher.NotifyEventAsync(@event);
                }
            }

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
