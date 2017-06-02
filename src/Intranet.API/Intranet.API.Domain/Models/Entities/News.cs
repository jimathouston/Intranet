using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Intranet.API.Domain.Models.Entities
{
    public class News
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        public string Text { get; set; }

        [Required]
        public string Author { get; set; }

        public ICollection<NewsTag> NewsTags { get; set; }
    }
}
