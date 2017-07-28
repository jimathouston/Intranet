using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Intranet.API.UnitTests.TestHelpers
{
    public static class ActionResultHelpers
    {
        public static int CountItems(this IActionResult actionResult)
        {
            return GetResponsesAs<object>(actionResult).Count();
        }

        public static IEnumerable<T> GetResponsesAs<T>(this IActionResult actionResult)
            where T : class
        {
            return GetResponseAs<IEnumerable<T>>(actionResult);
        }

        public static T GetResponseAs<T>(this IActionResult actionResult)
            where T : class
        {
            return actionResult.GetType().GetProperty("Value").GetValue(actionResult) as T;
        }
    }
}
