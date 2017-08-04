using Intranet.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Intranet.Web.Extensions
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
                .SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?
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
                .SingleOrDefault(c => c.Type.Equals(ClaimTypes.Name))?
                .Value;
        }

        /// <summary>
        /// Get the display name
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims
                .SingleOrDefault(c => c.Type.Equals(ClaimTypes.Email))?
                .Value;
        }

        #region NewsViewModel
        public static bool IsAllowedToModifyNews(this ClaimsPrincipal claimsPrincipal, NewsViewModel news)
        {
            return claimsPrincipal.IsAdmin() || news.UserId == claimsPrincipal.GetUsername();
        }
        #endregion
    }
}