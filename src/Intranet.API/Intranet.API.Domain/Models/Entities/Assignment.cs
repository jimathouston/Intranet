using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
    public class Assignment
    {
    public int EmployeeId { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee Employee { get; set; }

    public int ProjectId { get; set; }

    public Project Project { get; set; }

    public int LocationId { get; set; }

    public Location Location { get; set; }

    public int RoleId { get; set; }

    public Role Role { get; set; }

    public string InformalDescription { get; set; }

    public bool Active { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset EndDate { get; set; }
  }
}
