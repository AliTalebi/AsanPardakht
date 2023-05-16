using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Queries.Infrastructure.DomainModel.Entities
{
    public sealed class Person
    {
        public int Id { get; set; }
        [NotNull]
        public string? Name { get; set; }
        [NotNull]
        public string? NationalCode { get; set; }

        public HashSet<PersonAddress> Addresses { get; set; } = new();
    }
}