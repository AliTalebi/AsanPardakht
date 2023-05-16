using AsanPardakht.Core.Resources;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Infrastructure.Core.Resources
{
    public sealed class AspNetCoreResourceManager : IResourceManager
    {
        private readonly IStringLocalizer _localizedizer;
        public AspNetCoreResourceManager(IStringLocalizer stringLocalizer)
        {
            _localizedizer = stringLocalizer;
        }

        [NotNull]
        public string? this[string? name] => FindString(name)!;

        public string? FindString(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            var resource = _localizedizer[name];

            return resource.ResourceNotFound ? name : resource.Value;
        }
    }
}
