using AsanPardakht.Domain.Provinces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using AsanPardakht.Infrastructre.Persistence.EfCore.Views;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Repositories
{
    public sealed class ProvinceRepository : BaseRepository<Province, ProvinceId>
    {
        public ProvinceRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext) { }

        [NotNull]
        protected override string? SequenceName => "dbo.Provinces";

        public override async Task<Province?> GetByIdAsync(ProvinceId aggregateId, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<Province>().SingleOrDefaultAsync(x => x.Id == aggregateId, cancellationToken);
        }

        protected override ProvinceId ConvertToAggregateId(GeneratedNewIdView viewResult)
        {
            return new ProvinceId(viewResult.Id);
        }
    }
}