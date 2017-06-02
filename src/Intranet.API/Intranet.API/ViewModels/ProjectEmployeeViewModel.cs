using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.API.ViewModels
{
    public class ProjectEmployeeViewModel
    {
        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string InformalProjectDescription { get; set; }

        public string ClientName { get; set; }

        public int RoleId { get; set; }

        public string RoleDescription { get; set; }

        public bool Active { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }
    }
}
