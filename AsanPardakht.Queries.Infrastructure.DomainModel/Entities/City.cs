using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Queries.Infrastructure.DomainModel.Entities
{
    public sealed class City
    {
        public int Id { get; set; }
        [NotNull]
        public string? Name { get; set; }

        public int ProvinceId { get; set; }
        [NotNull]
        public Province? Province { get; set; }
    }
}
