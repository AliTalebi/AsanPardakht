using AutoMapper;
using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;

namespace AsanPardakht.Queries.Queries.People
{
    public sealed class PeopleMapProfile : Profile
    {
        public PeopleMapProfile()
        {
            CreateMap<Person, GetPersonByIdQueryResult>();
            CreateMap<Person, GetPeopleListQueryResult>();
        }
    }
}
