using AsanPardakht.Core.Domain;

namespace AsanPardakht.Domain.Provinces.DomainServices
{
    public interface IProvinceExistByNameDomainService : IDomainService
    {
        Task<bool> IsNameExistAsync(string? name, CancellationToken cancellationToken = default!);
    }
}