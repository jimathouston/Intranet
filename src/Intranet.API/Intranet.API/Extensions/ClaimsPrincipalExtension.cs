using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Intranet.API.Extensions
{
    /// <summary>
    /// Collection of helper methods for easy retrival of claims
    /// </summary>
    public static class ClaimsPrincipalExtension
    {
        /// <summary>
        /// Returns true if the user is Admin
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole("Admin");
        }

        /// <summary>
        /// Get the username
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public static string GetUsername(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims
                    .SingleOrDefault(c => c.Type.Equals("username", StringComparison.OrdinalIgnoreCase))?
                    .Value;
        }

        /// <summary>
        /// Get the display name
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public static string GetDisplayName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims
                    .SingleOrDefault(c => c.Type.Equals("displayName", StringComparison.OrdinalIgnoreCase))?
                    .Value;
        }
    }
}
