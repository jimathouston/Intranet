using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.API.ViewModels
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Mobile { get; set; }

        public string StreetAdress { get; set; }

        public int PostalCode { get; set; }

        public string City { get; set; }
    }
}
