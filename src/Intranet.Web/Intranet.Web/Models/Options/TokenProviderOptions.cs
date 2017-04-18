using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Intranet.Web.Models.Options
{
  public class TokenProviderOptions
  {
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);

    public SigningCredentials SigningCredentials { get; set; }
  }
}
