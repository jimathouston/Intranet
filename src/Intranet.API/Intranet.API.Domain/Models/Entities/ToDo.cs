using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
  public class ToDo
  {
    public int ChecklistId { get; set; }          

    public Checklist Checklist { get; set; }          

    public int EmployeeId { get; set; }      

    public Employee Employee { get; set; }   

    public bool Done { get; set; }
  }
}
