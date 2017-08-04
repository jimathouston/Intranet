using Intranet.Web.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Web.Domain.Contracts
{
    public interface IKeywordRelation
    {
        int KeywordId { get; set; }
        Keyword Keyword { get; set; }
    }
}
