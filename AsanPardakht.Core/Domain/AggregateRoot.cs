using AsanPardakht.Core.Domain.Events;

namespace AsanPardakht.Core.Domain
{
    public abstract class AggregateRoot<TAggregateRootId> : IAggregateRoot<TAggregateRootId> where TAggregateRootId : notnull
    {
        private readonly List<IDomainEvent> _events = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _events.AsReadOnly();

        public abstract TAggregateRootId Id { get; protected set; }

        protected virtual void AddEvent(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }
    }
}