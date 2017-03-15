using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Intranet.Web.Models;
using Intranet.Web.Contracts;

namespace Intranet.Web.Services
{
  public class AuthenticationService : IAuthenticationService
  {
    public IUser VerifyUser(string username, string password)
    {
      // TODO: Verify against an external service
      if (username != password)
      {
        return null;
      }

      return new User
      {
        FirstName = "Oskar",
        IsVerified = true,
        AuthenticationType = "password",
        Sid = "12345",
      };
    }
  }
}
