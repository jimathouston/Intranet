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
                new Claim("displayName", user.DisplayName),
                new Claim("username", user.Username)
            };

            if (user.IsAdmin)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else if (user.IsDeveloper)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, "Developer"));
            }

            var claimsIdentity = new ClaimsIdentity(userClaims, _authenticationService.GetType().Name);
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
