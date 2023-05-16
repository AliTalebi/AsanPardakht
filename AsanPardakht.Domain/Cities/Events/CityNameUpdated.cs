using AsanPardakht.Core.Domain.Events;

namespace AsanPardakht.Domain.Cities.Events
{
    public record struct CityNameUpdated(CityId Id, string? Name) : IDomainEvent;
}
