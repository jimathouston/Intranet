using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Intranet.Selenium.Framework.Enums;

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

        /// <summary>
        /// Find all IWebElements matching the specified identifier and return as Element object
        /// </summary>
        /// <param name="identifier">Identifier describing the element(s) to find</param>
        /// <param name="elementName">Name of IWebElement(s) for logging purposes</param>
        /// <returns>Element containing all IWebElements on current page matching identifier</returns>
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

        /// <summary>
        /// Wait for up to timeOut for at least one IWebElement on the current page to match identifier,
        /// then return all matching IWebElements as Element object
        /// </summary>
        /// <param name="identifier">Identifier describing the element(s) to find</param>
        /// <param name="elementName">Name of IWebElement(s) for logging purposes</param>
        /// <param name="timeOut">Time to wait in Milliseconds</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if at least one IWebElement on the current page matches the provided identifier,
        /// also saves any found elements though a reference variable
        /// </summary>
        /// <param name="identifier">identifier describing the element(s) to find</param>
        /// <param name="elements">List of IWebElements to save any matching elements to</param>
        /// <returns>True if at least one element matches identifier, otherwise false</returns>
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
