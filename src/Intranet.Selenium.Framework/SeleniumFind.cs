using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Selenium.Framework
{

    public class SeleniumFind
    {
        private readonly IWebDriver _driver;
        private readonly SeleniumLogger _logger;

        public SeleniumFind(IWebDriver driver, SeleniumLogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        public Element Elements(By identifier, string elementName)
        {
            _logger.Write($"FIND: Element(s) {elementName}");
            try
            {
                List<IWebElement> elements = new List<IWebElement>(_driver.FindElements(identifier));
                _logger.Write($"{elements.Count} matching Element(s) found");
                return new Element(elements, elementName);
            }
            catch (NoSuchElementException)
            {
                _logger.Write("0 matching Element(s) found", Level.Warn);
                return null;
            }
        }

        public Element Elements(By identifier, string elementName, int timeOut)
        {
            const int pollFreq = 100;
            int waited = 0;

            _logger.Write($"FIND: Element(s) {elementName} (timeOut: {timeOut}ms)");
            List<IWebElement> elements = new List<IWebElement>();

            while (!ElementExists(identifier, ref elements) && waited <= timeOut)
            {
                System.Threading.Thread.Sleep(pollFreq);
                waited += pollFreq;
            }

            if (elements.Count > 0)
            {
                _logger.Write($"{elements.Count} matching Element(s) found");
                return new Element(elements, elementName);
            }
            else
            {
                _logger.Write("0 matching Element(s) found within time limit", Level.Warn);
                return null;
            }
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
