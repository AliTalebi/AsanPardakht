using AsanPardakht.Core.Domain;

namespace AsanPardakht.Domain.Provinces.DomainServices
{
    public interface IProvinceExistByIdDomainService : IDomainService
    {
        Task<bool> IsProvinceExistAsync(ProvinceId provinceId, CancellationToken cancellationToken = default!);
    }
}
