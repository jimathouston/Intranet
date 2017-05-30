using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Intranet.Web.Contracts;

namespace Intranet.Web.Providers.Contracts
{
    public interface IAuthenticationProvider
    {
        ClaimsPrincipal GetClaimsPrincipal(IUser user);
    }
}
