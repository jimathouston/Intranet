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

        public void Click(Element element)
        {

        }

        public void Click(Element element, int xOffset, int yOffset)
        {

        }

        public void ClickJS(Element element)
        {

        }

        public void SendKeys(Element element, string input)
        {

        }

        public void SelectFromDropdown(Element element, string optionToSelect)
        {

        }
    }
}
