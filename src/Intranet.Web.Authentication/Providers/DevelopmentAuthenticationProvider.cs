using Intranet.Web.Authentication.Contracts;
using Intranet.Web.Authentication.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Intranet.Web.Authentication.Providers
{
    public class DevelopmentAuthenticationProvider : AuthenticationProvider, IAuthenticationProvider
    {
        private readonly IAuthenticationService _authenticationService;

        public DevelopmentAuthenticationProvider(IAuthenticationService authenticationService)
            : base(authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public new ClaimsPrincipal GetClaimsPrincipal(IUser user)
        {
            var claimsPrincipal = base.GetClaimsPrincipal(user);

            if (claimsPrincipal is null)
            {
                return null;
            }

            if (user.IsDeveloper && !claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                var userClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                };

                var claimsIdentity = new ClaimsIdentity(userClaims, _authenticationService.GetType().Name);

                claimsPrincipal.AddIdentity(claimsIdentity);
            }


            return claimsPrincipal;
        }
    }
}
