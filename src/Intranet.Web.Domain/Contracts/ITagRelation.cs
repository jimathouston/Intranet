using Intranet.Web.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Web.Domain.Contracts
{
    public interface ITagRelation
    {
        int TagId { get; set; }
        Tag Tag { get; set; }
    }
}
