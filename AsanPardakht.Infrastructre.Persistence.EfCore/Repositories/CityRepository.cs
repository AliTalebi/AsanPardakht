using AsanPardakht.Domain.Cities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using AsanPardakht.Infrastructre.Persistence.EfCore.Views;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Repositories
{
    public sealed class CityRepository : BaseRepository<City, CityId>
    {
        public CityRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext) { }

        [NotNull]
        protected override string? SequenceName => "dbo.Cities";

        protected override CityId ConvertToAggregateId(GeneratedNewIdView viewResult)
        {
            return new CityId(viewResult.Id);
        }

        public override async Task<City?> GetByIdAsync(CityId aggregateId, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<City>().SingleOrDefaultAsync(x => x.Id == aggregateId, cancellationToken);
        }
    }
}