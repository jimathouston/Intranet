using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Intranet.API.Domain.Models.Entities
{
  public class News
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Text { get; set; }

    public string Author { get; set; }
    
  }
}
