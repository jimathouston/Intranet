using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Selenium.Framework
{
    public class SeleniumNavigate
    {
        private readonly IWebDriver _driver;

        public SeleniumNavigate(IWebDriver driver)
        {
            _driver = driver;
        }

        public void GoToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }
    }
}
