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
    public class DevelopmentAuthenticationProvider_Fact
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
            var authenticationProvider = new DevelopmentAuthenticationProvider(authService);
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
            Assert.True(claimsPrincipal.Identity.IsAuthenticated);
            Assert.True(claimsPrincipal.Identity.AuthenticationType == authService.GetType().Name);
            Assert.True(claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == user.DisplayName));
            Assert.True(claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Username));
            Assert.True(claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Email && c.Value == user.Email));
            Assert.True(claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin"));
        }

        [Fact]
        public void When_Given_Non_Developer_Ignore_Role_Return_ClaimsPrincipal()
        {
            // Assign
            #region Options
            IOptions<LdapConfig> ldapOptions = Options.Create(new LdapConfig());
            #endregion
            #region Service
            var authService = new LdapAuthenticationService(ldapOptions);
            #endregion
            var authenticationProvider = new DevelopmentAuthenticationProvider(authService);
            var user = new User
            {
                DisplayName = "Calle Carlsson",
                Username = "calle.carlsson",
                Email = "calle.carlsson@gmail.com",
            };

            // Act
            var claimsPrincipal = authenticationProvider.GetClaimsPrincipal(user);

            // Assert
            Assert.False(claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Role));
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
            var authenticationProvider = new DevelopmentAuthenticationProvider(authService);
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
        public void When_Given_Admin_And_Dev_Return_Admin_ClaimsPrincipal()
        {
            // Assign
            #region Options
            IOptions<LdapConfig> ldapOptions = Options.Create(new LdapConfig());
            #endregion
            #region Service
            var authService = new LdapAuthenticationService(ldapOptions);
            #endregion
            var authenticationProvider = new DevelopmentAuthenticationProvider(authService);
            var user = new User
            {
                DisplayName = "Calle Carlsson",
                Username = "calle.carlsson",
                Email = "calle.carlsson@gmail.com",
                IsAdmin = true,
                IsDeveloper = true,
            };

            // Act
            var claimsPrincipal = authenticationProvider.GetClaimsPrincipal(user);

            // Assert
            Assert.True(claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin"));
        }
    }
}


