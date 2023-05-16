using OneOf;
using AutoMapper;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AsanPardakht.Queries.Persistence.EfCore.Data;
using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;

namespace AsanPardakht.Queries.Queries.Cities
{
    public record GetCityListQuery : IPagingBaseQuery<QueryResponse<List<GetCityListQueryResult>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
        public int? ProvinceId { get; set; }
    }

    public record GetCityListQueryResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ProvinceId { get; set; }
    }

    public sealed class GetCityListQueryHandler : IQueryHandler<GetCityListQuery, QueryResponse<List<GetCityListQueryResult>>>
    {
        private readonly QueryDbContext _dbContext;
        private readonly IConfigurationProvider _configurationProvider;

        public GetCityListQueryHandler(QueryDbContext queryDbContext, IConfigurationProvider configurationProvider)
        {
            _dbContext = queryDbContext;
            _configurationProvider = configurationProvider;
        }

        public async Task<OneOf<QueryResponse<List<GetCityListQueryResult>>, Error>> ExecuteAsync(GetCityListQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            var query = _dbContext.Set<City>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(x => x.Name.Contains(request.Name));
            }

            if (request.ProvinceId.HasValue)
            {
                query = query.Where(x => x.ProvinceId.Equals(request.ProvinceId));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var data = await query.OrderByDescending(x => x.Id).UsePaging(request.Page, request.PageSize)
                .ProjectTo<GetCityListQueryResult>(_configurationProvider).ToListAsync(cancellationToken);

            return PagingQueryResponse<List<GetCityListQueryResult>>.Create(totalCount, data);
        }
    }
}