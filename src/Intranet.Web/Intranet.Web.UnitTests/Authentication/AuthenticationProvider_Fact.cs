using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Intranet.Web.Models;
using Intranet.Web.Providers;
using Xunit;
using Intranet.Web.Authentication.Providers;
using Intranet.Web.Authentication.Models;
using Microsoft.Extensions.Options;
using Intranet.Web.Authentication.Services;

namespace Intranet.Web.UnitTests.Authentication
{
    public class AuthenticationProvider_Fact
    {
        [Fact]
        public void When_Given_User_Return_ClaimsPrincipal()
        {
            // Assign
            #region Options
            IOptions<LdapConfig> ldapOptions = Options.Create(new LdapConfig());
            #endregion
            #region Service
            var authService = new LdapAuthenticationService(ldapOptions);
            #endregion
            var authenticationProvider = new AuthenticationProvider(authService);
            var user = new User
            {
                DisplayName = "Calle Carlsson",
                Username = "calle.carlsson",
            };

            // Act
            var claimsPrincipal = authenticationProvider.GetClaimsPrincipal(user);
            var identity = claimsPrincipal.Identities.SingleOrDefault();
            var actClaims = claimsPrincipal.Claims.ToList();

            // Assert
            Assert.True(identity.IsAuthenticated);
            Assert.True(identity.AuthenticationType == authService.GetType().Name);
            Assert.True(actClaims.SingleOrDefault(m => m.Type == "displayName").Value == user.DisplayName);
            Assert.True(actClaims.SingleOrDefault(m => m.Type == "username").Value == user.Username);
        }
    }
}


