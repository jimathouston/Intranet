using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Intranet.API.UnitTests.Fakes
{
    public class ClaimsIdentityFake : ClaimsIdentity
    {
        public ClaimsIdentityFake(params Claim[] claims)
            : base(claims)
        {
            // Empty
        }
    }
}
