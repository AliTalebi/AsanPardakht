using AsanPardakht.Core.Data;
using AsanPardakht.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using AsanPardakht.Infrastructre.Persistence.EfCore.Views;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Repositories
{
    public abstract class BaseRepository<TAggregate, TAggregateId> : IRepository<TAggregate, TAggregateId>
        where TAggregate : IAggregateRoot<TAggregateId> where TAggregateId : notnull
    {
        protected ApplicationDbContext DbContext { get; init; }

        protected BaseRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public abstract Task<TAggregate?> GetByIdAsync(TAggregateId aggregateId, CancellationToken cancellationToken = default);

        public virtual void Insert(TAggregate aggregate)
        {
            DbContext.Add(aggregate);
        }

        public async Task<TAggregateId> GetNewIdAsync()
        {
            return ConvertToAggregateId(DbContext.Set<GeneratedNewIdView>().FromSqlRaw($"SELECT NEXT VALUE FOR {SequenceName} AS Id").AsEnumerable().SingleOrDefault()!);
        }

        [NotNull]
        protected abstract string? SequenceName { get; }
        protected abstract TAggregateId ConvertToAggregateId(GeneratedNewIdView viewResult);
    }
}
