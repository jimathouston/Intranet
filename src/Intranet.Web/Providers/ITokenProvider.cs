using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Intranet.Web.Providers
{
  public interface ITokenProvider
  {
    (string accessToken, int expiresIn) GenerateToken(ClaimsPrincipal user);
  }
}
