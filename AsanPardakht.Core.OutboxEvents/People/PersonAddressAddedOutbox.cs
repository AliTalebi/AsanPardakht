using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Core.OutboxEvents.People
{
    public class PersonAddressAddedOutbox : IOutboxEvent
    {
        public string? Id { get; set; }
        [NotNull]
        public string? Detail { get; set; }
        [NotNull]
        public string? CityName { get; set; }
        [NotNull]
        public string? ProvinceName { get; set; }
    }
}