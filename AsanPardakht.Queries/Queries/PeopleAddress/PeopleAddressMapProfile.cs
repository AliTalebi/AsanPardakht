using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;
using AutoMapper;

namespace AsanPardakht.Queries.Queries.PeopleAddress
{
    public sealed class PeopleAddressMapProfile : Profile
    {
        public PeopleAddressMapProfile()
        {
            CreateMap<PersonAddress, GetAddressListQueryResult>();

            CreateMap<City, GetAddressListCityQueryResult>();
            CreateMap<Province, GetAddressListProvinceQueryResult>();
        }
    }
}
