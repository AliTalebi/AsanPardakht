using AsanPardakht.Core.Domain.Events;

namespace AsanPardakht.Domain.People.Events
{
    public record struct PersonCreated(PersonId id, string? Name, string? NationalCode) : IDomainEvent;
}
