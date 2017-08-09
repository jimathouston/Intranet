using Intranet.Web.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class NewsTag : TagRelation, ITagRelation
    {
        public NewsTag()
        {
            // Empty
        }

        public NewsTag(Tag tag)
            : base(tag)
        {

        }

        public int NewsId { get; set; }
        public News News { get; set; }
    }
}
