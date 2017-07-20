using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
    public class NewsKeyword
    {
        public int NewsId { get; set; }
        public News News { get; set; }

        public string KeywordId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
