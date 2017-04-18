using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intranet.Web.Contracts;
using Intranet.Web.Models;

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
