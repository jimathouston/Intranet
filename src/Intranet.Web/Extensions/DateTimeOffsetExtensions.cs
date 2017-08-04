using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Web.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static string ToFullDateString(this DateTimeOffset dateTime)
        {
            return dateTime.ToString("D", new CultureInfo("en"));
        }
    }
}
