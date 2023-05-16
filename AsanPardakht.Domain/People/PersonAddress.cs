using AsanPardakht.Domain.Cities;
using AsanPardakht.Domain.Provinces;

namespace AsanPardakht.Domain.People
{
    public record PersonAddress
    {
        public PersonAddress(ProvinceId provinceId, CityId cityId, string? detail)
        {
            CityId = cityId;
            Detail = detail;
            ProvinceId = provinceId;
        }

        public string? Detail { get; init; }
        public CityId CityId { get; init; }
        public ProvinceId ProvinceId { get; init; }
    }
}
