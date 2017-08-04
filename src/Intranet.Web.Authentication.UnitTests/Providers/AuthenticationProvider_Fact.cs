using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;
using Intranet.Web.Authentication.Providers;
using Intranet.Web.Authentication.Models;
using Microsoft.Extensions.Options;
using Intranet.Web.Authentication.Services;

namespace Intranet.Web.Authentication.UnitTests.Providers
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
                Email = "calle.carlsson@gmail.com",
            };

            // Act
            var claimsPrincipal = authenticationProvider.GetClaimsPrincipal(user);
            var identity = claimsPrincipal.Identities.SingleOrDefault();
            var actClaims = claimsPrincipal.Claims.ToList();

            // Assert
            Assert.True(identity.IsAuthenticated);
            Assert.True(identity.AuthenticationType == authService.GetType().Name);
            Assert.True(actClaims.SingleOrDefault(m => m.Type == ClaimTypes.Name).Value == user.DisplayName);
            Assert.True(actClaims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value == user.Username);
            Assert.True(actClaims.SingleOrDefault(m => m.Type == ClaimTypes.Email).Value == user.Email);
        }

        [Fact]
        public void When_Given_Admin_Return_ClaimsPrincipal()
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
                Email = "calle.carlsson@gmail.com",
                IsAdmin = true,
            };

            // Act
            var claimsPrincipal = authenticationProvider.GetClaimsPrincipal(user);

            // Assert
            Assert.True(claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin"));
        }

        [Fact]
        public void When_Given_Developer_Ignore_Role_And_Return_ClaimsPrincipal()
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
                Email = "calle.carlsson@gmail.com",
                IsDeveloper = true,
            };

            // Act
            var claimsPrincipal = authenticationProvider.GetClaimsPrincipal(user);

            // Assert
            Assert.False(claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Role));
        }
    }
}


