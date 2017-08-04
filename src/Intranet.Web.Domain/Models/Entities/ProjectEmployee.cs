using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class ProjectEmployee
    {
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        [Required]
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public string InformalDescription { get; set; }

        public bool Active { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }
    }
}
