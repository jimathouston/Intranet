using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Selenium.Framework
{

    public class SeleniumFind
    {
        private readonly IWebDriver _driver;

        public SeleniumFind(IWebDriver driver)
        {
            _driver = driver;
        }

        public SeleniumElement Elements(By identifier, string elementName)
        {
            List<IWebElement> elements = new List<IWebElement>(_driver.FindElements(identifier));
            return new SeleniumElement(elements, elementName);
        }

        public SeleniumElement Elements(By identifier, string elementName, int timeOut)
        {
            const int pollFreq = 100;

            int waited = 0;
            List<IWebElement> elements = new List<IWebElement>();

            while (!ElementExists(identifier, ref elements) && waited <= timeOut)
            {
                System.Threading.Thread.Sleep(pollFreq);
                waited += pollFreq;
            }

            if (elements.Count > 0)
            {
                return new SeleniumElement(elements, elementName);
            }
            else throw new NoSuchElementException("No matching element found within specified time limit");
        }

        private bool ElementExists(By identifier, ref List<IWebElement> elements)
        {
            try
            {
                IReadOnlyList<IWebElement> foundElements = _driver.FindElements(identifier);
                elements.AddRange(foundElements);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
