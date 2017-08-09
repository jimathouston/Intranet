using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Interactions;

namespace Intranet.Selenium.Framework
{
    public class SeleniumInteract
    {
        private readonly IWebDriver _driver;
        private readonly SeleniumLogger _logger;

        public SeleniumInteract(IWebDriver driver, SeleniumLogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        public void Click(Element element)
        {
            Click(element.FirstOrDefault(), element.Name);
        }

        public void Click(IWebElement element, string name)
        {
            _logger.Write($"INTERACT: Click Element {name}");
            try
            {
                element.Click();
                _logger.Write($"Action Completed");
            }
            catch (Exception e) when (e is InvalidElementStateException || e is ElementNotVisibleException || 
                e is StaleElementReferenceException || e is NoSuchElementException)
            {
                string exception = e.GetType().ToString();
                _logger.Write($"Element {name} could not be clicked: {exception}", Level.Error);
            }
        }

        public void Click(Element element, int xOffset, int yOffset)
        {
            Click(element.FirstOrDefault(), element.Name, xOffset, yOffset);
        }

        public void Click(IWebElement element, string name, int xOffset, int yOffset)
        {
            _logger.Write($"INTERACT: Click Element {name} with offset X: {xOffset}, Y: {yOffset}");
            Actions builder = new Actions(_driver);
            try
            {
                IAction clickWithOffset = builder
                    .MoveToElement(element, xOffset, yOffset)
                    .Click()
                    .Build();
                clickWithOffset.Perform();
                _logger.Write("Action Completed");
            }
            catch (Exception e) when (e is InvalidElementStateException || e is ElementNotVisibleException || 
                e is StaleElementReferenceException || e is NoSuchElementException)
            {
                string exception = e.GetType().ToString();
                _logger.Write($"Element {name} could not be clicked: {exception}", Level.Error);
            }
        }

        public void ClickJS(Element element)
        {
            ClickJS(element.FirstOrDefault(), element.Name);
        }

        public void ClickJS(IWebElement element, string name)
        {
            _logger.Write($"INTERACT: Click Element {name} using JavaScript");
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click()", element);
                _logger.Write("Action Completed");
            }
            catch (Exception e)
            {
                string exception = e.GetType().ToString();
                _logger.Write($"Element {name} could not be clicked: {exception}", Level.Error);
            }
        }

        public void SendKeys(Element element, string input)
        {
            SendKeys(element.FirstOrDefault(), element.Name, input);
        }

        public void SendKeys(IWebElement element, string name, string input)
        {
            _logger.Write($"INTERACT: Send Input to Element {name}: '{input}'");
            try
            {
                element.SendKeys(input);
                _logger.Write("Action Completed");
            }
            catch (Exception e) when (e is InvalidElementStateException || e is ElementNotVisibleException || 
                e is StaleElementReferenceException || e is NoSuchElementException)
            {
                string exception = e.GetType().ToString();
                _logger.Write($"Input could not be sent to Element {name}: {exception}", Level.Error);
            }
        }
    }
}
