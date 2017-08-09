using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
