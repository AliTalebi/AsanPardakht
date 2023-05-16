using AsanPardakht.Core.Security;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AsanPardakht.Infrastructure.Core.Security
{
    public sealed class AspNetCoreUserIdentityAccessor : IUserIdentityAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AspNetCoreUserIdentityAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserName()
        {
            var userNameClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name);

            if (userNameClaim == null)
                return "ali.talebi";

            return userNameClaim.Value;
        }

        public string? GetNationalCode()
        {
            var userNameClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("national_code");

            if (userNameClaim == null)
                return "0320565076";

            return userNameClaim.Value;
        }

        public int GetSubjectId()
        {
            var userNameClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Sid);

            if (userNameClaim == null)
                return 1;

            _ = int.TryParse(userNameClaim.Value, out int subjectId);

            return subjectId;
        }
    }
}
