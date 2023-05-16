using OneOf;
using AutoMapper;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Security;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AsanPardakht.Queries.Persistence.EfCore.Data;
using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;

namespace AsanPardakht.Queries.Queries.PeopleAddress
{
    public record GetAddressListQuery : IPagingBaseQuery<QueryResponse<List<GetAddressListQueryResult>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string? CityName { get; set; }
    }

    public record GetAddressListQueryResult
    {
        public int Id { get; set; }
        public string? Detail { get; set; }

        public GetAddressListCityQueryResult? City { get; set; }
        public GetAddressListProvinceQueryResult? Province { get; set; }
    }

    public record GetAddressListCityQueryResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public record GetAddressListProvinceQueryResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }


    public sealed class GetAddressListQueryHandler : IQueryHandler<GetAddressListQuery, QueryResponse<List<GetAddressListQueryResult>>>
    {
        private readonly QueryDbContext _dbContext;
        private readonly IUserIdentityAccessor _userIdentityAccessor;
        private readonly IConfigurationProvider _configurationProvider;

        public GetAddressListQueryHandler(QueryDbContext queryDbContext, IConfigurationProvider configurationProvider, IUserIdentityAccessor userIdentityAccessor)
        {
            _dbContext = queryDbContext;
            _userIdentityAccessor = userIdentityAccessor;
            _configurationProvider = configurationProvider;
        }

        public async Task<OneOf<QueryResponse<List<GetAddressListQueryResult>>, Error>> ExecuteAsync(GetAddressListQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }
            var nationalCode = _userIdentityAccessor.GetNationalCode();

            var query = _dbContext.Set<Person>().AsQueryable()
                .Where(x => x.NationalCode.Equals(nationalCode)).SelectMany(x => x.Addresses);

            if (!string.IsNullOrWhiteSpace(request.CityName))
            {
                query = query.Where(x => x.City.Name.Equals(request.CityName));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var data = await query.OrderByDescending(x => x.Id).UsePaging(request.Page, request.PageSize)
                .ProjectTo<GetAddressListQueryResult>(_configurationProvider).ToListAsync(cancellationToken);

            return PagingQueryResponse<List<GetAddressListQueryResult>>.Create(totalCount, data);
        }
    }
}