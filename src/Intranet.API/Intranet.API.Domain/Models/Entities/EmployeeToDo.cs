using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
  public class EmployeeToDo
  {
    [Required]
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; }

    [Required]
    public int ToDoId { get; set; }          

    public ToDo ToDo { get; set; }
    
    public bool Done { get; set; }
  }
}
