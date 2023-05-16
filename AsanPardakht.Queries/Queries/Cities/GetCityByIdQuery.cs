using OneOf;
using AutoMapper;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;
using AsanPardakht.Queries.Persistence.EfCore.Data;

namespace AsanPardakht.Queries.Queries.Cities
{
    public record GetCityByIdQuery(int Id) : IBaseQuery<QueryResponse<GetCityByIdQueryResult>>;
    public record GetCityByIdQueryResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ProvinceId { get; set; }
    }

    public sealed class GetCityByIdQueryHandler : IQueryHandler<GetCityByIdQuery, QueryResponse<GetCityByIdQueryResult>>
    {
        private readonly QueryDbContext _dbContext;
        private readonly IConfigurationProvider _configurationProvider;
        public GetCityByIdQueryHandler(QueryDbContext queryDbContext, IConfigurationProvider configurationProvider)
        {
            _dbContext = queryDbContext;
            _configurationProvider = configurationProvider;
        }

        public async Task<OneOf<QueryResponse<GetCityByIdQueryResult>, Error>> ExecuteAsync(GetCityByIdQuery? request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            var city = await _dbContext.Set<City>()
              .ProjectTo<GetCityByIdQueryResult>(_configurationProvider)
              .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return QueryResponse<GetCityByIdQueryResult>.Create(city);
        }
    }
}