using AsanPardakht.Core.Domain.Events;

namespace AsanPardakht.Core.Domain
{
    public interface IAggregateRoot
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
    }

    public interface IAggregateRoot<TAgregateRootId> : IEntity<TAgregateRootId>, IAggregateRoot
        where TAgregateRootId : notnull
    {
    }
}