using Intranet.Web.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public abstract class KeywordRelation : IKeywordRelation
    {
        public KeywordRelation()
        {
            // Empty
        }

        public KeywordRelation(Keyword keyword)
        {
            Keyword = keyword;
        }

        public int KeywordId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
