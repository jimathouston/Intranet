using Intranet.Web.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class Faq : IHasTags
    {
        public int Id { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        public ICollection<FaqTag> FaqTags { get; set; }

        public string Url { get; set; }
    }
}
