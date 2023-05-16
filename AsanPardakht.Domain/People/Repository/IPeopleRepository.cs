using AsanPardakht.Core.Data;

namespace AsanPardakht.Domain.People.Repository
{
    public interface IPeopleRepository : IRepository<Person, PersonId>
    {
        Task<Person?> GetByNationalCodeAsync(string? nationalcode, CancellationToken cancellationToken = default!);
    }
}
