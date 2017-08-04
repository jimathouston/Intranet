using Intranet.Web.Authentication.Contracts;
using Intranet.Web.Authentication.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Intranet.Web.Authentication.Providers
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationProvider(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public ClaimsPrincipal GetClaimsPrincipal(IUser user)
        {
            if (user is null)
            {
                return null;
            }

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (user.IsAdmin)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var claimsIdentity = new ClaimsIdentity(userClaims, _authenticationService.GetType().Name);
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
