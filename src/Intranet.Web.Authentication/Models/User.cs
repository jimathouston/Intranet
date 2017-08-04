using Intranet.Web.Authentication.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Web.Authentication.Models
{
    public class User : IUser
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeveloper { get; set; }
    }
}
