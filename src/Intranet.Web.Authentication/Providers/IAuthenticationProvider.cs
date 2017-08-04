using Intranet.Web.Authentication.Contracts;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Intranet.Web.Authentication.Providers
{
    public interface IAuthenticationProvider
    {
        ClaimsPrincipal GetClaimsPrincipal(IUser user);
    }
}
