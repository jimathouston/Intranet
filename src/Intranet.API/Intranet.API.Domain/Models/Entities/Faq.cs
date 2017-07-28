using Intranet.API.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
    public class Faq : IHasKeywords
    {
        public int Id { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        public ICollection<FaqKeyword> FaqKeywords { get; set; }

        public string Url { get; set; }
    }
}
