using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Intranet.Web.Extensions
{
    public static class HostingEnviromentlExtensions
    {
        public static bool IsE2e(this IHostingEnvironment env)
        {
            return env.EnvironmentName.Equals("e2e", StringComparison.OrdinalIgnoreCase);
        }
    }
}
