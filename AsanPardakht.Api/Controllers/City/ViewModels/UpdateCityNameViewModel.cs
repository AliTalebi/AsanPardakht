using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Api.Controllers.City.ViewModels
{
    public sealed class UpdateCityNameViewModel
    {
        [Required]
        [NotNull]
        public string? Name { get; set; }
    }
}
