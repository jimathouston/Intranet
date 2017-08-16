using Intranet.Selenium.Framework.Enums;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Selenium.Framework
{
    public interface ISeleniumDriver
    {
        IWebDriver DriverComponent { get; }
        IResultTracker TestOutcome { get; }

        void Log(string message);
        void Log(string message, Level level);
        void SaveScreenshot(string screenshotName);
        void CloseBrowser();
    }
}
