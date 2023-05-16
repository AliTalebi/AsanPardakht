using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Queries.Infrastructure.DomainModel.Entities
{
    public class Province
    {
        public int Id { get; set; }
        [NotNull]
        public string? Name { get; set; }
    }
}
