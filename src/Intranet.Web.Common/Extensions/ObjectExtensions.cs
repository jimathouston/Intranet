using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Web.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull<T>(this T obj)
              where T : class
        {
            return (obj == null);
        }

        public static bool IsNotNull<T>(this T obj)
              where T : class
        {
            return (obj != null);
        }
    }
}
