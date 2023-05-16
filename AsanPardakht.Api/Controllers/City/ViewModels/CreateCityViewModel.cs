using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Api.Controllers.City.ViewModels
{
    public sealed class CreateCityViewModel
    {
        [Required]
        [NotNull]
        public string? Name { get; set; }

        [Required]
        public int ProvinceId { get; set; }


    }
}
