using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
    }
}
