using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace Intranet.Selenium.Framework
{
    public class SeleniumElement
    {
        public List<IWebElement> Elements { get; set; }
        public string Name { get; set; }

        public SeleniumElement (IWebElement element, string elementName)
        {
            Elements = new List<IWebElement>
            {
                element
            };
            Name = elementName;
        }

        public SeleniumElement (List<IWebElement> elements, string elementName)
        {
            Elements = new List<IWebElement>(elements);
            Name = elementName;
        }
    }
}
