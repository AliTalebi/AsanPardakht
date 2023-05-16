using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp.Options
{
    public sealed class ApiOptions
    {
        public const string SECTION_NAME = nameof(ApiOptions);

        [Required]
        [NotNull]
        public string? BaseAddress { get; set; }

        [Required]
        [NotNull]
        public string? Api { get; set; }
        
        [Required]
        [NotNull]
        public string? Credential { get; set; }
    }
}
