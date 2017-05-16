using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
  public class SkillLevel
  {
    public int Id { get; set; }

    [Required]
    public string Description { get; set; }

    [InverseProperty("Desired")]
    public ICollection<Skill> DesiredSkillLevels { get; set; }

    [InverseProperty("Current")]
    public ICollection<Skill> CurrentSkillLevels { get; set; }

  }
}
