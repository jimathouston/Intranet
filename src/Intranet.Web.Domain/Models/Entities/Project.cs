using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        public int LocationId { get; set; }

        public Location Location { get; set; }
    }
}
