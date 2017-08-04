using Intranet.Web.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class PolicyKeyword : KeywordRelation, IKeywordRelation
    {
        public PolicyKeyword()
        {
            // Empty
        }

        public PolicyKeyword(Keyword keyword)
            : base(keyword)
        {
            // Empty
        }

        public int PolicyId { get; set; }
        public Policy Policy { get; set; }
    }
}
