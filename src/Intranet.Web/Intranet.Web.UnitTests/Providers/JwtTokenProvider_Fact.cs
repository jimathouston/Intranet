using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Intranet.Shared.Factories;
using Intranet.Web.Models;
using Intranet.Web.Models.Options;
using Intranet.Web.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft;
using Newtonsoft.Json;
using Xunit;
using Intranet.Web.Authentication.Models;
using Intranet.Web.Authentication.Providers;
using Intranet.Web.Authentication.Services;

namespace Intranet.Web.UnitTests
{
    public class JwtTokenProvider_Fact
    {
        [Fact]
        public void When_Given_ClaimsPrincipal_Return_JWT()
        {
            // Assign
            #region TokenProviderOptions
            var secretKey = "A very secret key123!!!";
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var audience = "TestAudience";
            var issuer = "TestIssuer";
            var expiration = TimeSpan.FromMinutes(5);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            IOptions<TokenProviderOptions> tokenProviderOptions = Options.Create
            (
              new TokenProviderOptions
              {
                  Audience = audience,
                  Expiration = expiration,
                  Issuer = issuer,
                  SigningCredentials = signingCredentials,
              }
            );
            #endregion
            #region DateTimeFactoryMock
            var dateTimeOffset = new DateTimeOffset(2017, 02, 10, 11, 11, 12, TimeSpan.FromMinutes(0));
            var dateTimeFactoryMock = new Mock<IDateTimeFactory>();
            dateTimeFactoryMock.Setup(m => m.DateTimeOffset).Returns(dateTimeOffset);
            #endregion
            #region ClaimsPrincipal
            var user = new User
            {
                DisplayName = "Oskar",
                IsAdmin = true,
                Username = "oskar",
            };

            var authOptions = Options.Create(new LdapConfig());
            var authService = new LdapAuthenticationService(authOptions);
            var authProvider = new AuthenticationProvider(authService);
            var userClaimsPrincipal = authProvider.GetClaimsPrincipal(user);
            #endregion
            #region JwtSecurityTokenHandler
            var handler = new JwtSecurityTokenHandler();
            #endregion

            var jwtTokenProvider = new JwtTokenProvider(tokenProviderOptions, dateTimeFactoryMock.Object);

            // Act
            var encodedToken = jwtTokenProvider.GenerateToken(userClaimsPrincipal);
            var jwt = handler.ReadJwtToken(encodedToken.accessToken);

            // Assert
            Assert.True(jwt.Claims.SingleOrDefault(c => c.Type == "displayName").Value == "Oskar");
            Assert.True(jwt.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat).Value == dateTimeOffset.ToUnixTimeSeconds().ToString());
        }
    }
}
