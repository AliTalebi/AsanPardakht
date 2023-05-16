using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Queries.Infrastructure.DomainModel.Entities
{
    public sealed class PersonAddress
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int PersonId { get; set; }
        public int ProvinceId { get; set; }
        [NotNull]
        public string? Detail { get; set; }

        [NotNull]
        public City? City { get; set; }
        [NotNull]
        public Province? Province { get; set; }
        
        [NotNull]
        public Person? Person { get; set; }
    }
}
