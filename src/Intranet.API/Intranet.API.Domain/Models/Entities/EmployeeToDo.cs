using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
  public class EmployeeToDo
  {
    public int ToDoId { get; set; }          

    public ToDo ToDo { get; set; }          

    public int EmployeeId { get; set; }      

    public Employee Employee { get; set; }   

    public bool Done { get; set; }
  }
}
