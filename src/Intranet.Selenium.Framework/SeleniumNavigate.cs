using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Selenium.Framework
{
    public class SeleniumNavigate
    {
        private readonly IWebDriver _driver;
        private readonly SeleniumLogger _logger;

        public SeleniumNavigate(IWebDriver driver, SeleniumLogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        public void GoToUrl(string url)
        {
            _logger.Write($"NAVIGATE: Go to URL: '{url}'");
            _driver.Navigate().GoToUrl(url);
            _logger.Write("Navigation Successful");
        }

        public void Refresh()
        {
            _logger.Write("NAVIGATE: Refresh");
            _driver.Navigate().Refresh();
            _logger.Write("Navigation Successful");
        }

        public void GoBack()
        {
            _logger.Write("NAVIGATE: Back");
            _driver.Navigate().Back();
            _logger.Write("Navigation Successful");
        }

        public void GoForward()
        {
            _logger.Write("NAVIGATE: Forward");
            _driver.Navigate().Forward();
            _logger.Write("Navigation Successful");
        }

        public void ScrollToElement(Element target)
        {
            ScrollToElement(target.FirstOrDefault(), target.Name);
        }

        public void ScrollToElement(IWebElement target, string name)
        {
            _logger.Write($"NAVIGATE: Scroll to Element {name}");
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true)", target);
                _logger.Write("Navigation Successful");
            }
            catch (Exception e)
            {
                string exception = e.GetType().ToString();
                _logger.Write($"Navigation failed: {exception}", Level.Error);
            }
        }
    }
}
