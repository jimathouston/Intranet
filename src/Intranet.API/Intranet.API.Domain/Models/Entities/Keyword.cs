using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
    public class Keyword
    {
        [Key]
        public string Name { get; set; }
        public ICollection<NewsKeyword> NewsKeyword { get; set; }

        public override string ToString() => Name;
    }
}
