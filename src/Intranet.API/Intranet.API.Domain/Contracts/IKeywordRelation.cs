using Intranet.API.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.API.Domain.Contracts
{
    public interface IKeywordRelation
    {
        int KeywordId { get; set; }
        Keyword Keyword { get; set; }
    }
}
