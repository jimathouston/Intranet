using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Web.ViewModels
{
    public class TextEditorViewModel
    {
        public int Height { get; set; } = 250;
        public bool Inline { get; set; } = false;
        public string Id { get; set; }
        public string Text { get; set; }
    }
}
