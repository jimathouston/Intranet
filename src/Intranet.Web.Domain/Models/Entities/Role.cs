using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
    }
}
