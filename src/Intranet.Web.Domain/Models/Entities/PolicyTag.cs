using Intranet.Web.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public class PolicyTag : TagRelation, ITagRelation
    {
        public PolicyTag()
        {
            // Empty
        }

        public PolicyTag(Tag tag)
            : base(tag)
        {
            // Empty
        }

        public int PolicyId { get; set; }
        public Policy Policy { get; set; }
    }
}
