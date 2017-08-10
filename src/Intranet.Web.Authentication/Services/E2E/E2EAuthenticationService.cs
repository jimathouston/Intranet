using Intranet.Web.Authentication.Contracts;
using Intranet.Web.Authentication.Models;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;

namespace Intranet.Web.Authentication.Services.E2E
{
    public class E2EAuthenticationService : IAuthenticationService
    {
        private readonly LdapConfig _config;

        public E2EAuthenticationService(IOptions<LdapConfig> config)
        {
            _config = config.Value;
        }

        public IUser VerifyUser(string username, string password)
        {
            
            if (username == "admin" && password == "password1")
            {
                return new User
                {
                    DisplayName = "John Doe (Admin)",
                    Username = "john.doe",
                    Email = "john.doe@email.com",
                    IsAdmin = true,
                    IsDeveloper = false,
                };
            }
            else if (username == "test" && password == "password1")
            {
                return new User
                {
                    DisplayName = "John Doe",
                    Username = "john.doe",
                    Email = "john.doe@email.com",
                    IsAdmin = false,
                    IsDeveloper = false,
                };
            }
            return null;
        }
    }
}
