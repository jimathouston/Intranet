using Intranet.Web.Common.Extensions;
using Intranet.Web.Models.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Web.Middlwares
{
    public class ProtectFolderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly PathString _path;
        private readonly string _policyName;

        public ProtectFolderMiddleware(RequestDelegate next, ProtectFolderOptions options)
        {
            _next = next;
            _path = options.Path;
            _policyName = options.PolicyName;
        }

        public async Task Invoke(HttpContext httpContext, IAuthorizationService authorizationService)
        {
            if (httpContext.Request.Path.StartsWithSegments(_path))
            {
                var authorized = _policyName.IsNull()
                    ? httpContext.User.Identity.IsAuthenticated 
                    : (await authorizationService.AuthorizeAsync(httpContext.User, null, _policyName)).Succeeded;

                if (!authorized)
                {
                    await httpContext.ChallengeAsync();
                    return;
                }
            }

            await _next(httpContext);
        }
    }
}
