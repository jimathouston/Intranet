using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Intranet.Shared.Factories;
using Intranet.Web.Models.Options;
using Intranet.Web.Providers.Contracts;
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

        public (string accessToken, int expiresIn) GenerateToken(ClaimsPrincipal user)
        {
            var now = _dateTimeFactory.DateTimeOffset;

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            if (user.HasClaim(c => c.Type == "displayName"))
            {
                claims.Add(user.Claims.SingleOrDefault(c => c.Type == "displayName"));
            }

            if (user.HasClaim(c => c.Type == "username"))
            {
                claims.Add(user.Claims.SingleOrDefault(c => c.Type == "username"));
            }

            if (user.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                var roleClaim = new Claim("role", user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role).Value);
                claims.Add(roleClaim);
            }

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

            return
            (
                accessToken: encodedJwt,
                expiresIn: (int)_settings.Expiration.TotalSeconds
            );
        }
    }
}
