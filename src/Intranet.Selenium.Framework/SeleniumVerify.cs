using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Intranet.Selenium.Framework.Enums;

namespace Intranet.Selenium.Framework
{
    public class SeleniumVerify
    {
        private readonly IWebDriver _driver;
        private readonly SeleniumLogger _logger;

        public SeleniumVerify(IWebDriver driver, SeleniumLogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        /// <summary>
        /// Returns true if at least one IWebElement on the current page matches the provided identifier
        /// </summary>
        /// <param name="identifier">identifier describing the element(s) to find</param>
        /// <returns>True if at least one element matches identifier, otherwise false</returns>
        #pragma warning disable S1481 // Assigning to unused variable necessary to carry out intended function ot method
        public bool ElementExists(By identifier, string name = "Element")
        {
            _logger.Write($"VERIFY: {name} exists");
            try
            {
                IWebElement foundElements = _driver.FindElement(identifier);
                _logger.Write($"{name} is present");
                return true;
            }
            catch (NoSuchElementException)
            {
                _logger.Write($"No {name} found");
                return false;
            }
        }
    }
}
