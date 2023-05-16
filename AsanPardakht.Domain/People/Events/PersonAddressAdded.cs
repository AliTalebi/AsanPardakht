using AsanPardakht.Domain.Cities;
using AsanPardakht.Domain.Provinces;
using AsanPardakht.Core.Domain.Events;

namespace AsanPardakht.Domain.People.Events
{
    public record PersonAddressAdded(PersonId Id, ProvinceId ProvinceId, string? ProvinceName, CityId CityId, string? CityName, string? Detail) : IDomainEvent;
}
