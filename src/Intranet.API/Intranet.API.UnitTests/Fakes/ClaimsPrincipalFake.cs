using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Intranet.API.UnitTests.Fakes
{
    public class ClaimsPrincipalFake : ClaimsPrincipal
    {
        public ClaimsPrincipalFake(params Claim[] claims)
            : base(new ClaimsIdentityFake(claims))
        {
            // Fake
        }
    }
}
