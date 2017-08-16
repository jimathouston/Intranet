using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Interactions;
using Intranet.Selenium.Framework.Enums;

namespace Intranet.Selenium.Framework
{
    public static class Interact
    {
        /// <summary>
        /// Click the specified Element
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="driver">Target webdriver</param>
        public static void Click(Element element, ISeleniumDriver driver)
        {
            Click(element.FirstOrDefault(), element.Name, driver);
        }

        /// <summary>
        /// Click the specified IWebElement
        /// </summary>
        /// <param name="element">Target IWebelement</param>
        /// <param name="name">Name of element (For logging purposes)</param>
        /// <param name="driver">Target webdriver</param>
        public static void Click(IWebElement element, string name, ISeleniumDriver driver)
        {
            driver.Log($"INTERACT: Click Element {name}");
            try
            {
                element.Click();
                driver.Log($"Action Completed");
            }
            catch (Exception e) when (e is InvalidElementStateException || e is ElementNotVisibleException || 
                e is StaleElementReferenceException || e is NoSuchElementException || e is NullReferenceException)
            {
                string exception = e.GetType().ToString();
                driver.Log($"Element {name} could not be clicked: {exception}", Level.Error);
            }
        }

        /// <summary>
        /// Click the specified Element, with an offset
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="xOffset">Horizontal offset</param>
        /// <param name="yOffset">Vertical offset</param>
        /// <param name="driver">Target webdriver</param>
        public static void Click(Element element, int xOffset, int yOffset, ISeleniumDriver driver)
        {
            Click(element.FirstOrDefault(), element.Name, xOffset, yOffset, driver);
        }

        /// <summary>
        /// Click the specified IWebElement, with and offset
        /// </summary>
        /// <param name="element">Target IWebElement</param>
        /// <param name="name">name of element (for logging purposes)</param>
        /// <param name="xOffset">Horizontal offset</param>
        /// <param name="yOffset">Vertical offset</param>
        /// <param name="driver">Target webdriver</param>
        public static void Click(IWebElement element, string name, int xOffset, int yOffset, ISeleniumDriver driver)
        {
            driver.Log($"INTERACT: Click Element {name} with offset X: {xOffset}, Y: {yOffset}");
            Actions builder = new Actions(driver.DriverComponent);
            try
            {
                IAction clickWithOffset = builder
                    .MoveToElement(element, xOffset, yOffset)
                    .Click()
                    .Build();
                clickWithOffset.Perform();
                driver.Log("Action Completed");
            }
            catch (Exception e) when (e is InvalidElementStateException || e is ElementNotVisibleException || 
                e is StaleElementReferenceException || e is NoSuchElementException || e is NullReferenceException)
            {
                string exception = e.GetType().ToString();
                driver.Log($"Element {name} could not be clicked: {exception}", Level.Error);
            }
        }

        /// <summary>
        /// Emulate a Click action using JavaScript - only use if regular click does not register properly
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="driver">Target webdriver</param>
        public static void ClickJS(Element element, ISeleniumDriver driver)
        {
            ClickJS(element.FirstOrDefault(), element.Name, driver);
        }

        /// <summary>
        /// Emulate a Click action using JavaScript - only use if regular click does not register properly
        /// </summary>
        /// <param name="element">Target IWebElement</param>
        /// <param name="name">name of element (for logging purposes)</param>
        /// <param name="driver">Target webdriver</param>
        public static void ClickJS(IWebElement element, string name, ISeleniumDriver driver)
        {
            driver.Log($"INTERACT: Click Element {name} using JavaScript");
            try
            {
                ((IJavaScriptExecutor)driver.DriverComponent).ExecuteScript("arguments[0].click()", element);
                driver.Log("Action Completed");
            }
            catch (Exception e)
            {
                string exception = e.GetType().ToString();
                driver.Log($"Element {name} could not be clicked: {exception}", Level.Error);
            }
        }

        /// <summary>
        /// Send key input to element
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="input">Input to send</param>
        /// <param name="driver">Target webdriver</param>
        public static void SendKeys(Element element, string input, ISeleniumDriver driver)
        {
            SendKeys(element.FirstOrDefault(), element.Name, input, driver);
        }

        /// <summary>
        /// Semd key input to IWebElement
        /// </summary>
        /// <param name="element">Target IWebElement</param>
        /// <param name="name">Name fo element (for logging purposes)</param>
        /// <param name="input">Input to send</param>
        /// <param name="driver">Target webdriver</param>
        public static void SendKeys(IWebElement element, string name, string input, ISeleniumDriver driver)
        {
            driver.Log($"INTERACT: Send Input to Element {name}: '{input}'");
            try
            {
                element.SendKeys(input);
                driver.Log("Action Completed");
            }
            catch (Exception e) when (e is InvalidElementStateException || e is ElementNotVisibleException || 
                e is StaleElementReferenceException || e is NoSuchElementException || e is NullReferenceException)
            {
                string exception = e.GetType().ToString();
                driver.Log($"Input could not be sent to Element {name}: {exception}", Level.Error);
            }
        }
    }
}
