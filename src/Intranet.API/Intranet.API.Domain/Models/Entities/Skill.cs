using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
    public class Skill
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<EmployeeSkill> EmployeeSkills { get; set; }
    }
}
