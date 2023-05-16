using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace AsanPardakht.Api.Controllers.Province.ViewModels
{
    public sealed class UpdateProvinceNameViewModel
    {
        [Required]
        [NotNull]
        public string? Name { get; set; }
    }
}
