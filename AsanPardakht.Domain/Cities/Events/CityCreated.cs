using AsanPardakht.Core.Domain.Events;
using AsanPardakht.Domain.Provinces;

namespace AsanPardakht.Domain.Cities.Events
{
    public record struct CityCreated(CityId Id, string? Name, ProvinceId ProvinceId) : IDomainEvent;
}
