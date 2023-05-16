using AsanPardakht.Core.Domain;

namespace AsanPardakht.Domain.Cities.DomainServices
{
    public interface ICityExistByNameDomainService : IDomainService
    {
        Task<bool> IsNameExistAsync(string? name, CancellationToken cancellationToken = default!);
    }
}