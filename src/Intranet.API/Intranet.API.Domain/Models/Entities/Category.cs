using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Intranet.API.Domain.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Url { get; set; }

        public ICollection<Faq> Faqs { get; set; }

        /// <summary>
        /// NOTE: This method requires all relations to be eager loaded to work!
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool HasNoRelatedEntities(object obj)
        {
            return Faqs.Contains(obj) && Faqs.Count == 1;
        }
    }
}