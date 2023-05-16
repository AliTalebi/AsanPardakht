using AsanPardakht.Domain.People;
using AsanPardakht.Domain.People.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.DomainServices.People
{
    public sealed class PersonExistByNationalCodeDomainService : IPersonExistByNationalCodeDomainService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PersonExistByNationalCodeDomainService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> IsNationalCodeExistAsync(string? nationalCode, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Set<Person>().AnyAsync(x => x.NationalCode.Equals(nationalCode), cancellationToken);
        }
    }
}
