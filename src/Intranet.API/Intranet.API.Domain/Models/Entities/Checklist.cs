using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
  public class Checklist
  {
    public int ChecklistId { get; set; }

    [Required]
    public string Description { get; set; }

    public ICollection<ToDo> ToDos { get; set; }
  }
}
