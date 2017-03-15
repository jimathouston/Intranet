using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Intranet.Web.Models;
using Intranet.Web.Contracts;

namespace Intranet.Web.Provider
{
  public class AuthenticationProvider : IAuthenticationProvider
  {
    public ClaimsPrincipal GetClaimsPrincipal(IUser user)
    {
      if (!user.IsVerified)
      {
        return null;
      }

      var claims = new List<Claim>
      {
          new Claim("Read", user.IsVerified.ToString().ToLower()),
          new Claim(ClaimTypes.Surname, user.FirstName),
          new Claim(ClaimTypes.Sid, user.Sid),
      };

      var claimsIdentity = new ClaimsIdentity(claims, user.AuthenticationType);
      return new ClaimsPrincipal(claimsIdentity);
    }
  }
}
