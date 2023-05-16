using OneOf;
using AutoMapper;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AsanPardakht.Queries.Persistence.EfCore.Data;
using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;

namespace AsanPardakht.Queries.Queries.People
{
    public record GetPeopleListQuery : IPagingBaseQuery<QueryResponse<List<GetPeopleListQueryResult>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
        public string? NationalCode { get; set; }
    }

    public record GetPeopleListQueryResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NationalCode { get; set; }
    }

    public sealed class GetPeopleListQueryHandler : IQueryHandler<GetPeopleListQuery, QueryResponse<List<GetPeopleListQueryResult>>>
    {
        private readonly QueryDbContext _dbContext;
        private readonly IConfigurationProvider _configurationProvider;
        public GetPeopleListQueryHandler(QueryDbContext queryDbContext, IConfigurationProvider configurationProvider)
        {
            _dbContext = queryDbContext;
            _configurationProvider = configurationProvider;
        }

        public async Task<OneOf<QueryResponse<List<GetPeopleListQueryResult>>, Error>> ExecuteAsync(GetPeopleListQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            var query = _dbContext.Set<Person>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(x => x.Name.Contains(request.Name));
            }

            if (!string.IsNullOrWhiteSpace(request.NationalCode))
            {
                query = query.Where(x => x.NationalCode.Contains(request.NationalCode));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var data = await query.OrderByDescending(x => x.Id).UsePaging(request.Page, request.PageSize)
                .ProjectTo<GetPeopleListQueryResult>(_configurationProvider).ToListAsync(cancellationToken);

            return PagingQueryResponse<List<GetPeopleListQueryResult>>.Create(totalCount, data);
        }
    }
}