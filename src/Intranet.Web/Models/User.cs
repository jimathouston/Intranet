using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intranet.Web.Contracts;

namespace Intranet.Web.Models
{
  public class User : IUser
  {
    public string FirstName { get; set; }
    public string AuthenticationType { get; internal set; }
    public bool IsVerified { get; internal set; } = false;
    public string Sid { get; internal set; }
  }
}
