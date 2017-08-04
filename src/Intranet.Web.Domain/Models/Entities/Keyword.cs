using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class Keyword
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public ICollection<NewsKeyword> NewsKeywords { get; set; }
        public ICollection<FaqKeyword> FaqKeywords { get; set; }
        public ICollection<PolicyKeyword> PolicyKeywords { get; set; }

        public override string ToString() => Name;
    }
}
