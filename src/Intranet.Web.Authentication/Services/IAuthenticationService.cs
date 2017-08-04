using Intranet.Web.Authentication.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Web.Authentication.Services
{
    public interface IAuthenticationService
    {
        IUser VerifyUser(string username, string password);
    }
}
