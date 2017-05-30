using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
    public class Skill
    {
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int SkillTypeId { get; set; }

        public SkillType SkillType { get; set; }

        public int CurrentSkillLevelId { get; set; }

        public SkillLevel Current { get; set; }

        public int DesiredSkillLevelId { get; set; }

        public SkillLevel Desired { get; set; }

    }
}
