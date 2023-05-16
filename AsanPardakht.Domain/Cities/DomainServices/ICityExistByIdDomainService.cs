using AsanPardakht.Core.Domain;

namespace AsanPardakht.Domain.Cities.DomainServices
{
    public interface ICityExistByIdDomainService : IDomainService
    {
        Task<bool> IsCityExistAsync(CityId cityId, CancellationToken cancellationToken = default!);
    }
}
