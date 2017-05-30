using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
    public class SkillType
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<Skill> Skills { get; set; }
    }
}
