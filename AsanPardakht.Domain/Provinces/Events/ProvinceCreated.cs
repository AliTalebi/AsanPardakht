using AsanPardakht.Core.Domain.Events;

namespace AsanPardakht.Domain.Provinces.Events
{
    public record struct ProvinceCreated(ProvinceId Id, string? Name) : IDomainEvent;
}
