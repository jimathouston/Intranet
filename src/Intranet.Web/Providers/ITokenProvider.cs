using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Intranet.Web.Providers
{
  public interface ITokenProvider
  {
    object GenerateToken(ClaimsPrincipal user);
  }
}
