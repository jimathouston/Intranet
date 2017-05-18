using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
  public class Location
  {
    public int Id { get; set; }

    [Required]
    public string Description { get; set; }

    public int Coordinate { get; set; }

    public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
  }
}
