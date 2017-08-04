using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Intranet.Web.Domain.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Url { get; set; }

        public ICollection<Faq> Faqs { get; set; }
        public ICollection<Policy> Policies { get; set; }
    }
}