using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Core.Data
{
    public sealed class OutBoxEventData
    {
        public long Id { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool Read { get; set; }
        [NotNull]
        public string? Data { get; set; }
        [NotNull]
        public string? IssuedBy { get; set; }
        [NotNull]
        public string? EventType { get; set; }
        public DateTime IssuedAt { get; set; }
        [NotNull]
        public string? AggregateId { get; set; }
        [NotNull]
        public string? AggregateType { get; set; }
    }
}