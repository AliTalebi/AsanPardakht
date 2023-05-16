using AsanPardakht.Domain.Provinces;
using AsanPardakht.Domain.Provinces.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.DomainServices.Provinces
{
    public sealed class ProvinceExistByIdDomainService : IProvinceExistByIdDomainService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ProvinceExistByIdDomainService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> IsProvinceExistAsync(ProvinceId ProvinceId, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Set<Province>().AsNoTracking().AnyAsync(x => x.Id == ProvinceId, cancellationToken);
        }
    }
}
