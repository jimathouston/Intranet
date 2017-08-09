using Intranet.Web.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class Policy : IHasTags
    {
        public int Id { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<PolicyTag> PolicyTags { get; set; }

        public string FileUrl { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
