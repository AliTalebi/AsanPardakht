using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace AsanPardakht.Api.Controllers.PeopleAddress.ViewModels
{
    public sealed class CreatePersonAddressViewModel
    {
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        [Required]
        [NotNull]
        public string? Detail { get; set; }
    }
}