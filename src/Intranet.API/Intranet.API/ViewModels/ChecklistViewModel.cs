using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.API.ViewModels
{
    public class ChecklistViewModel
    {
        public int ToDoId { get; set; }

        public int EmployeeId { get; set; }

        public string Description { get; set; }

        public bool Done { get; set; }
    }
}
