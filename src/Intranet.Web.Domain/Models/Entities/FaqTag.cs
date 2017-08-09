using Intranet.Web.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class FaqTag : TagRelation, ITagRelation
    {
        public FaqTag()
        {
            // Empty
        }

        public FaqTag(Tag tag)
            : base(tag)
        {
            // Empty
        }

        public int FaqId { get; set; }
        public Faq Faq { get; set; }
    }
}
