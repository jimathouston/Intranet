using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Intranet.Selenium.Framework.Enums;

namespace Intranet.Selenium.Framework
{
    public static class Navigate
    {
        /// <summary>
        /// Instruct the webdriver to go to a specific URL
        /// </summary>
        /// <param name="url">URL to navigate to</param>
        /// <param name="driver">Target webdriver</param>
        public static void GoToUrl(string url, ISeleniumDriver driver)
        {
            driver.Log($"NAVIGATE: Go to URL: '{url}'");
            driver.DriverComponent.Navigate().GoToUrl(url);
            driver.Log("Navigation Successful");
        }

        /// <summary>
        /// Instruct the webdriver to refresh the current page
        /// </summary>
        /// <param name="driver">Target webdriver</param>
        public static void Refresh(ISeleniumDriver driver)
        {
            driver.Log("NAVIGATE: Refresh");
            driver.DriverComponent.Navigate().Refresh();
            driver.Log("Navigation Successful");
        }

        /// <summary>
        /// Instruct the webdriver to go to the previous page in the browsing history
        /// </summary>
        /// <param name="driver">Target webdriver</param>
        public static void GoBack(ISeleniumDriver driver)
        {
            driver.Log("NAVIGATE: Back");
            driver.DriverComponent.Navigate().Back();
            driver.Log("Navigation Successful");
        }

        /// <summary>
        /// Instruct the webdriver to go the next page in the browsing history
        /// </summary>
        /// <param name="driver">Target webdriver</param>
        public static void GoForward(ISeleniumDriver driver)
        {
            driver.Log("NAVIGATE: Forward");
            driver.DriverComponent.Navigate().Forward();
            driver.Log("Navigation Successful");
        }

        /// <summary>
        /// Scroll the current page until the specified element is in view
        /// </summary>
        /// <param name="target">Element to scroll into view</param>
        /// <param name="driver">Target webdriver</param>
        public static void ScrollToElement(Element target, ISeleniumDriver driver)
        {
            ScrollToElement(target.FirstOrDefault(), target.Name, driver);
        }

        /// <summary>
        /// Scroll the current page until the specified element is in view
        /// </summary>
        /// <param name="target">Element to scroll into view</param>
        /// <param name="name">Name of element (For logging purposes)</param>
        /// <param name="driver">Target webdriver</param>
        public static void ScrollToElement(IWebElement target, string name, ISeleniumDriver driver)
        {
            driver.Log($"NAVIGATE: Scroll to Element {name}");
            try
            {
                ((IJavaScriptExecutor)driver.DriverComponent).ExecuteScript("arguments[0].scrollIntoView(true)", target);
                driver.Log("Navigation Successful");
            }
            catch (Exception e)
            {
                string exception = e.GetType().ToString();
                driver.Log($"Navigation failed: {exception}", Level.Error);
            }
        }
    }
}
