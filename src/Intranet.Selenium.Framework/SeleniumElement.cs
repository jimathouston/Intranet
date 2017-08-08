using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace Intranet.Selenium.Framework
{
    public class SeleniumElement
    {
        public IWebElement Element { get; set; }
        public string Name { get; set; }

        public SeleniumElement (IWebElement element, string elementName)
        {
            Element = element;
            Name = elementName;
        }
    }
}
