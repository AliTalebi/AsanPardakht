using AsanPardakht.Domain.Cities;
using Microsoft.EntityFrameworkCore;
using AsanPardakht.Domain.Cities.DomainServices;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.DomainServices.Cities
{
    public sealed class CityExistByNameDomainService : ICityExistByNameDomainService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CityExistByNameDomainService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> IsNameExistAsync(string? name, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Set<City>().AnyAsync(x => x.Name.Equals(name), cancellationToken);
        }
    }
}