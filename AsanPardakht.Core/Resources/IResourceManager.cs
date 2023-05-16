using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Core.Resources
{
    public interface IResourceManager
    {
        [NotNull]
        public string? this[string? name] { get; }

        string? FindString(string? name);
    }
}