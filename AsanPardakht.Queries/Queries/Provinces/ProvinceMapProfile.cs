using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;
using AutoMapper;

namespace AsanPardakht.Queries.Queries.Provinces
{
    public sealed class ProvinceMapProfile : Profile
    {
        public ProvinceMapProfile()
        {
            CreateMap<Province, GetProvinceByIdQueryResult>();
            CreateMap<Province, GetProvinceListQueryResult>();
        }
    }
}
