using Intranet.API.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
    public class NewsKeyword : KeywordRelation, IKeywordRelation
    {
        public NewsKeyword()
        {
            // Empty
        }

        public NewsKeyword(Keyword keyword)
            : base(keyword)
        {

        }

        public int NewsId { get; set; }
        public News News { get; set; }
    }
}
