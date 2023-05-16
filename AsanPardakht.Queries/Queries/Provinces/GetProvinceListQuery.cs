using OneOf;
using AutoMapper;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AsanPardakht.Queries.Persistence.EfCore.Data;
using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;

namespace AsanPardakht.Queries.Queries.Provinces
{
    public record GetProvinceListQuery : IPagingBaseQuery<QueryResponse<List<GetProvinceListQueryResult>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
    }

    public record GetProvinceListQueryResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public sealed class GetProvinceListQueryHandler : IQueryHandler<GetProvinceListQuery, QueryResponse<List<GetProvinceListQueryResult>>>
    {
        private readonly QueryDbContext _dbContext;
        private readonly IConfigurationProvider _configurationProvider;

        public GetProvinceListQueryHandler(QueryDbContext queryDbContext, IConfigurationProvider configurationProvider)
        {
            _dbContext = queryDbContext;
            _configurationProvider = configurationProvider;
        }

        public async Task<OneOf<QueryResponse<List<GetProvinceListQueryResult>>, Error>> ExecuteAsync(GetProvinceListQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            var query = _dbContext.Set<Province>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(x => x.Name.Contains(request.Name));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var data = await query.OrderByDescending(x => x.Id).UsePaging(request.Page, request.PageSize)
                .ProjectTo<GetProvinceListQueryResult>(_configurationProvider).ToListAsync(cancellationToken);

            return PagingQueryResponse<List<GetProvinceListQueryResult>>.Create(totalCount, data);
        }
    }
}