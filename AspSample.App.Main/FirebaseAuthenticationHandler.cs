using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FirebaseAdmin.Auth;

namespace AspSample.App.Main
{
    public class FirebaseAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
    }

    public class FirebaseAuthenticationHandler : AuthenticationHandler<FirebaseAuthenticationSchemeOptions>
    {
        public FirebaseAuthenticationHandler
        (
            IOptionsMonitor<FirebaseAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.NoResult();
            }

            var header = (string)Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(header))
            {
                return AuthenticateResult.NoResult();
            }
 
            if (!header.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
 
            string token = header.Substring("bearer".Length).Trim();
 
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
 
            try
            {
                return await validateToken(token);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }

        private async Task<AuthenticateResult> validateToken(string token)
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

            string uid = decodedToken.Uid;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, uid),
            };
 
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
