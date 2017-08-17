using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Intranet.Selenium.Framework
{
    public class Element
    {
        public IList<IWebElement> Elements { get; set; }
        public string Name { get; set; }

        public Element (IWebElement element, string elementName)
        {
            Elements = new List<IWebElement>
            {
                element
            };
            Name = elementName;
        }

        public Element (IList<IWebElement> elements, string elementName)
        {
            Elements = new List<IWebElement>(elements);
            Name = elementName;
        }

        public IWebElement FirstOrDefault()
        {
            return Elements.FirstOrDefault();
        }
    }
}
