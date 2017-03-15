using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Intranet.Web.Factories;
using Intranet.Web.Models;
using Microsoft.Extensions.Options;

namespace Intranet.Web.Providers
{
  public class JwtTokenProvider : ITokenProvider
  {
    private readonly TokenProviderOptions _settings;
    private readonly IDateTimeFactory _dateTimeFactory;

    public JwtTokenProvider(IOptions<TokenProviderOptions> settingsOptions,
                            IDateTimeFactory dateTimeFactory)
    {
      _settings = settingsOptions.Value;
      _dateTimeFactory = dateTimeFactory;
    }

    public object GenerateToken(ClaimsPrincipal user)
    {
      var now = _dateTimeFactory.GetCurrentDateTimeOffset();

      // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
      // You can add other claims here, if you want:
      var claims = new List<Claim>
      {
        new Claim("Read", true.ToString().ToLower()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
      };

      if (user.HasClaim(c => c.Type == ClaimTypes.Surname)) claims.Add(user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname));

      // Create the JWT
      var jwt = new JwtSecurityToken(
        issuer: _settings.Issuer,
        audience: _settings.Audience,
        claims: claims,
        notBefore: now.DateTime,
        expires: now.DateTime.Add(_settings.Expiration),
        signingCredentials: _settings.SigningCredentials
      );

      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      return new
      {
        accessToken = encodedJwt,
        expiresIn = (int)_settings.Expiration.TotalSeconds
      };
    }
  }
}
