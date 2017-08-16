using System;
using System.Collections.Generic;
using System.Text;
using Intranet.Selenium.Framework.Enums;

namespace Intranet.Selenium.Framework
{
    public interface IResultTracker
    {
        void PassStep();
        void FailStep();
        void Evaluate();
    }
}

