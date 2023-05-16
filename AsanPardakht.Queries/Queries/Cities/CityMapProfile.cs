using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;
using AutoMapper;

namespace AsanPardakht.Queries.Queries.Cities
{
    public sealed class CityMapProfile : Profile
    {
        public CityMapProfile()
        {
            CreateMap<City, GetCityByIdQueryResult>();
            CreateMap<City, GetCityListQueryResult>();
        }
    }
}
