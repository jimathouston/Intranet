using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Selenium.Framework
{
    public class SeleniumInteract
    {
        private readonly IWebDriver _driver;

        public SeleniumInteract(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}
