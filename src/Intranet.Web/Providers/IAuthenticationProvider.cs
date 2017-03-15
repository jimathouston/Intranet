using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Intranet.Web.Models;
using Intranet.Web.Contracts;

namespace Intranet.Web.Provider
{
  public interface IAuthenticationProvider
  {
    ClaimsPrincipal GetClaimsPrincipal(IUser user);
  }
}
