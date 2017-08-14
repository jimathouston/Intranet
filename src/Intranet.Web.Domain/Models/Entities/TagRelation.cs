using Intranet.Web.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Intranet.Web.Domain.Models.Entities
{
    public abstract class TagRelation : ITagRelation
    {
        public TagRelation()
        {
            // Empty
        }

        public TagRelation(Tag tag)
        {
            Tag = tag;
        }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
