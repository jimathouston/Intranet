using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public string Coordinate { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
