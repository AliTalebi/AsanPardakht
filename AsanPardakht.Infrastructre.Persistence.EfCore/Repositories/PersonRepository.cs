using AsanPardakht.Domain.People;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using AsanPardakht.Infrastructre.Persistence.EfCore.Views;
using AsanPardakht.Domain.People.Repository;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Repositories
{
    public sealed class PersonRepository : BaseRepository<Person, PersonId>, IPeopleRepository
    {
        public PersonRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext) { }

        [NotNull]
        protected override string? SequenceName => "dbo.People";

        public override async Task<Person?> GetByIdAsync(PersonId aggregateId, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<Person>().Include("_addresses").SingleOrDefaultAsync(x => x.Id == aggregateId, cancellationToken);
        }

        public async Task<Person?> GetByNationalCodeAsync(string? nationalcode, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<Person>().Include("_addresses").SingleOrDefaultAsync(x => x.NationalCode.Equals(nationalcode), cancellationToken);
        }

        protected override PersonId ConvertToAggregateId(GeneratedNewIdView viewResult)
        {
            return new PersonId(viewResult.Id);
        }
    }
}