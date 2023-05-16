using OneOf;
using AutoMapper;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AsanPardakht.Queries.Persistence.EfCore.Data;
using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;

namespace AsanPardakht.Queries.Queries.People
{
    public record GetPersonByIdQuery(int Id) : IBaseQuery<QueryResponse<GetPersonByIdQueryResult>>;
    public record GetPersonByIdQueryResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NationalCode { get; set; }
    }

    public sealed class GetPersonByIdQueryHandler : IQueryHandler<GetPersonByIdQuery, QueryResponse<GetPersonByIdQueryResult>>
    {
        private readonly QueryDbContext _dbContext;
        private readonly IConfigurationProvider _configurationProvider;
        public GetPersonByIdQueryHandler(QueryDbContext queryDbContext, IConfigurationProvider configurationProvider)
        {
            _dbContext = queryDbContext;
            _configurationProvider = configurationProvider;
        }

        public async Task<OneOf<QueryResponse<GetPersonByIdQueryResult>, Error>> ExecuteAsync(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            var person = await _dbContext.Set<Person>()
              .ProjectTo<GetPersonByIdQueryResult>(_configurationProvider)
              .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return QueryResponse<GetPersonByIdQueryResult>.Create(person);
        }
    }
}