using AsanPardakht.Core.Domain;

namespace AsanPardakht.Core.Data
{
    public interface IRepository { }
    public interface IRepository<TAggregate, TAggregateId> : IRepository
        where TAggregate : IAggregateRoot<TAggregateId>
        where TAggregateId : notnull
    {
        void Insert(TAggregate aggregate);
        Task<TAggregateId> GetNewIdAsync();
        Task<TAggregate?> GetByIdAsync(TAggregateId aggregateId, CancellationToken cancellationToken = default!);
    }
}
