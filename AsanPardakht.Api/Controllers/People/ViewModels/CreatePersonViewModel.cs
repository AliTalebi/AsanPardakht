using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace AsanPardakht.Api.Controllers.People.ViewModels
{
    public sealed class CreatePersonViewModel
    {
        [Required]
        [NotNull]
        public string? Name { get; set; }
        [Required]
        [NotNull]
        public string? NationalCode { get; set; }
    }
}