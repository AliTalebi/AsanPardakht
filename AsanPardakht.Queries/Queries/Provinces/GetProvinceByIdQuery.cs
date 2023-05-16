using OneOf;
using AutoMapper;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AsanPardakht.Queries.Persistence.EfCore.Data;
using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;

namespace AsanPardakht.Queries.Queries.Provinces
{
    public record GetProvinceByIdQuery(int Id) : IBaseQuery<QueryResponse<GetProvinceByIdQueryResult>>;
    public record GetProvinceByIdQueryResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public sealed class GetProvinceByIdQueryHandler : IQueryHandler<GetProvinceByIdQuery, QueryResponse<GetProvinceByIdQueryResult>>
    {
        private readonly QueryDbContext _dbContext;
        private readonly IConfigurationProvider _configurationProvider;
        public GetProvinceByIdQueryHandler(QueryDbContext queryDbContext, IConfigurationProvider configurationProvider)
        {
            _dbContext = queryDbContext;
            _configurationProvider = configurationProvider;
        }

        public async Task<OneOf<QueryResponse<GetProvinceByIdQueryResult>, Error>> ExecuteAsync(GetProvinceByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            var province = await _dbContext.Set<Province>()
                .ProjectTo<GetProvinceByIdQueryResult>(_configurationProvider)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return QueryResponse<GetProvinceByIdQueryResult>.Create(province);
        }
    }
}