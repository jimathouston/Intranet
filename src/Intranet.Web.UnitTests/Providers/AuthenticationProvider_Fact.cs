using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Intranet.Web.Models;
using Intranet.Web.Provider;
using Xunit;

namespace Intranet.Web.UnitTests
{
  public class AuthenticationProvider_Fact
  {
    [Fact]
    public void When_Given_User_Return_ClaimsPrincipal()
    {
      // Assign
      var authenticationProvider = new AuthenticationProvider();
      var user = new User
      {
        AuthenticationType = "password",
        FirstName = "Calle",
        IsVerified = true,
        Sid = "32143",
      };

      // Act
      var claimsPrincipal = authenticationProvider.GetClaimsPrincipal(user);
      var identity = claimsPrincipal.Identities.SingleOrDefault();
      var actClaims = claimsPrincipal.Claims.ToList();

      // Assert
      Assert.True(identity.IsAuthenticated);
      Assert.True(identity.AuthenticationType == "password");
      Assert.True(actClaims.SingleOrDefault(m => m.Type == "Read").Value == "true");
      Assert.True(actClaims.SingleOrDefault(m => m.Type == ClaimTypes.Surname).Value == user.FirstName);
      Assert.True(actClaims.SingleOrDefault(m => m.Type == ClaimTypes.Sid).Value == user.Sid);
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void When_Given_Unverified_User_Return_Null(bool isVerified, bool expectedResult)
    {
      // Assign
      var authenticationProvider = new AuthenticationProvider();
      var user = new User
      {
        FirstName = "Calle",
        IsVerified = isVerified,
        Sid = "32143",
      };

      // Act
      var claimsPrincipal = authenticationProvider.GetClaimsPrincipal(user);

      // Assert
      Assert.Equal(claimsPrincipal == null, expectedResult);
    }
  }
}
