using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class EmployeeSkill
    {
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int SkillId { get; set; }

        public Skill Skill { get; set; }

        [Required]
        public int CurrentLevel { get; set; }

        [ForeignKey(nameof(CurrentLevel))]
        public SkillLevel Current { get; set; }

        //[Required]
        //public int DesiredLevel { get; set; }

        //[ForeignKey(nameof(DesiredLevel))]
        //public SkillLevel Desired { get; set; }
       

    }
}
