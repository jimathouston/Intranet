using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Web.Factories
{
    public class DateTimeFactory : IDateTimeFactory
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public DateTimeOffset GetCurrentDateTimeOffset()
        {
            return DateTimeOffset.Now;
        }
    }
}
