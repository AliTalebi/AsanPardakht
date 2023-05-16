using AsanPardakht.Core.Domain;

namespace AsanPardakht.Domain.People.DomainServices
{
    public interface IPersonExistByNationalCodeDomainService : IDomainService
    {
        Task<bool> IsNationalCodeExistAsync(string? nationalCode, CancellationToken cancellationToken = default!);
    }
}