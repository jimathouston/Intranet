using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
  public class Employee
  {
    public int Id { get; set; }     // TODO Q: Should represent AD-id or just a generated Db-id?

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string Description { get; set; }

    [Required]
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Mobile { get; set; }

    public string StreetAdress { get; set; }

    public int PostalCode { get; set; }

    public string City { get; set; }

    public ICollection<ToDo> ToDos { get; set; }
  }
}
