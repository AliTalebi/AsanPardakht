using AsanPardakht.Core.Domain.Events;

namespace AsanPardakht.Domain.People.Events
{
    public record struct PersonChanged(PersonId id, string? Name, string? NationalCode) : IDomainEvent;
}
