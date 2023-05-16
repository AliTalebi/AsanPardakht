using AsanPardakht.Core.Domain.Events;

namespace AsanPardakht.Domain.Provinces.Events
{
    public record struct ProvinceNameUpdated(ProvinceId Id, string? Name) : IDomainEvent;
}
