using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intranet.Web.Contracts;

namespace Intranet.Web.Services
{
    public interface IAuthenticationService
    {
        IUser VerifyUser(string username, string password);
    }
}
