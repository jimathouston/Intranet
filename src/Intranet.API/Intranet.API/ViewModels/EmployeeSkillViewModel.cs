using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.API.ViewModels
{
  public class EmployeeSkillViewModel
  {
    public int EmployeeId { get; set; }

    public int SkillId { get; set; }

    public string SkillDescription { get; set; }

    public int CurrentId { get; set; }

    public string CurrentDescription { get; set; }

    public int DesiredId { get; set; }

    public string DesiredDescription { get; set; }
  }
}
