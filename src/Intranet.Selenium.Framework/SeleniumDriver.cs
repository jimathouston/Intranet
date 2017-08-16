using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using Microsoft.DotNet.PlatformAbstractions;
using System;
using Intranet.Selenium.Framework.Enums;

namespace Intranet.Selenium.Framework
{
    public class SeleniumDriver : ISeleniumDriver
    {
        public IWebDriver DriverComponent { get; }
        public IResultTracker TestOutcome { get; }

        private readonly ISeleniumLogger _logger;

        public SeleniumDriver(Browser browser, AssertLevel assertLevel, ISeleniumLogger logger)
        {
            _logger = logger;
            TestOutcome = new ResultTrackerXUnit(AssertLevel.Soft, _logger);

            switch (browser)
            {
                case Browser.Chrome:
                    {
                        DriverComponent = new ChromeDriver(ApplicationEnvironment.ApplicationBasePath);
                        break;
                    }
                case Browser.Firefox:
                    {
                        DriverComponent = null;
                        // Firefox temporary disabled as webdriver does not support specifying the path to the driver.
                        throw new NotImplementedException("Firefox Driver not implemented as driver does not support .NetCore");
                    }
                case Browser.InternetExplorer:
                    {
                        InternetExplorerOptions options = new InternetExplorerOptions()
                        {
                            EnableNativeEvents = true,  // Required for Clicking, Dragging and Dropping to work.
                            RequireWindowFocus = true,  // Required for Clicking, Dragging and Dropping to work.
                            IgnoreZoomLevel = true      // Required as ieDriver will otherwise only function at 100% zoom.
                        };
                        DriverComponent = new InternetExplorerDriver(ApplicationEnvironment.ApplicationBasePath, options);
                        break;
                    }
                case Browser.PhantomJS:
                    {
                        DriverComponent = new PhantomJSDriver(ApplicationEnvironment.ApplicationBasePath);
                        break;
                    }
                default:
                    {
                        DriverComponent = null;
                        throw new ArgumentOutOfRangeException($"No entry matching '{browser}' found in Enum 'Browser'");
                    }
            }
        }

        public void Log(string message)
        {
            _logger.Write(message);
        }

        public void Log(string message, Level level)
        {
            _logger.Write(message, level);
        }

        public void SaveScreenshot(string screenshotName)
        {
            _logger.SaveScreenshot(screenshotName, this);
        }

        public void CloseBrowser()
        {
            DriverComponent.Quit();
        }
    }
}
