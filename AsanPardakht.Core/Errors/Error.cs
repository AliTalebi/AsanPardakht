using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Core.Errors
{
    public record struct Error(string? Message, int Code = 0);
}
