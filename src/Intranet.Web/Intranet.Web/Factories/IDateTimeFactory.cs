using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Web.Factories
{
    public interface IDateTimeFactory
    {
        DateTime GetCurrentDateTime();
        DateTimeOffset GetCurrentDateTimeOffset();
    }
}
