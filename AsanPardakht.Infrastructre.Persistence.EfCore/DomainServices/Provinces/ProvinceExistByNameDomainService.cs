using AsanPardakht.Domain.Provinces;
using Microsoft.EntityFrameworkCore;
using AsanPardakht.Domain.Provinces.DomainServices;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.DomainServices.Provinces
{
    public sealed class ProvinceExistByNameDomainService : IProvinceExistByNameDomainService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ProvinceExistByNameDomainService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> IsNameExistAsync(string? name, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Set<Province>().AnyAsync(x => x.Name.Equals(name), cancellationToken);
        }
    }
}