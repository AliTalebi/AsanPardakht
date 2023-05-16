using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace AsanPardakht.Api.Security
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (authorizationHeader != null && authorizationHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                var token = authorizationHeader.Substring("Basic ".Length).Trim();

                var credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));

                var credentials = credentialsAsEncodedString.Split(':');

                if (credentials.Length == 2)
                {
                    var userName = credentials[0];
                    var password = credentials[1];

                    if (userName.Equals("ali.talebi") && password.Equals("123"))
                    {
                        var claims = new[]
                        {
                              new Claim(ClaimTypes.Sid, "1")
                            , new Claim(ClaimTypes.Name, userName)
                            , new Claim(ClaimTypes.GivenName, "ali")
                            , new Claim(ClaimTypes.Surname, "talebi")
                            , new Claim("national_code", "0320565076")
                            , new Claim(ClaimTypes.Email, "alitalebikondori@gmail.com")
                        };

                        var identity = new ClaimsIdentity(claims, "Basic");
                        var claimsPrincipal = new ClaimsPrincipal(identity);

                        return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                    }
                }
            }

            return await Task.FromResult(AuthenticateResult.Fail("authentication failed"));
        }
    }
}
