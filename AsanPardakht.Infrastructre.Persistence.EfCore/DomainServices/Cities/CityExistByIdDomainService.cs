using AsanPardakht.Domain.Cities;
using Microsoft.EntityFrameworkCore;
using AsanPardakht.Domain.Cities.DomainServices;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.DomainServices.Cities
{
    public sealed class CityExistByIdDomainService : ICityExistByIdDomainService
    {
        private readonly ApplicationDbContext _context;
        public CityExistByIdDomainService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<bool> IsCityExistAsync(CityId cityId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<City>().AsNoTracking().AnyAsync(x => x.Id == cityId, cancellationToken);
        }
    }
}
