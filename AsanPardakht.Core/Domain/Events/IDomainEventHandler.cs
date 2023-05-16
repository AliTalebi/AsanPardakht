namespace AsanPardakht.Core.Domain.Events
{
    public interface IDomainEventHandler { }
    public interface IDomainEventHandler<TDomainEvent> : IDomainEventHandler where TDomainEvent : IDomainEvent
    {
        Task ExecuteAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default!);
    }
}
