using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using Microsoft.DotNet.PlatformAbstractions;
using System;

namespace Intranet.Selenium.Framework
{
    public enum Browser
    {
        Firefox,
        Chrome,
        InternetExplorer,
        PhantomJS
    }

    public class SeleniumDriver
    {
        private readonly IWebDriver _driver;

        public SeleniumNavigate Navigate { get; }
        public SeleniumFind Find { get; }
        public SeleniumVerify Verify { get; }
        public SeleniumInteract Interact { get; }
        public SeleniumLogger Log { get; }

        public SeleniumDriver(Browser browser, string testName)
        {
            switch (browser)
            {
                case Browser.Chrome:
                    {
                        _driver = new ChromeDriver(ApplicationEnvironment.ApplicationBasePath);
                        break;
                    }
                case Browser.Firefox:
                    {
                        _driver = null;
                        // Firefox temporary disabled as webdriver does not support specifying the path to the driver.
                        throw new NotImplementedException("Firefox Driver not yet implemented");
                    }
                case Browser.InternetExplorer:
                    {
                        InternetExplorerOptions options = new InternetExplorerOptions()
                        {
                            EnableNativeEvents = true,  // Required for Clicking, Dragging and Dropping to work.
                            RequireWindowFocus = true,  // Required for Clicking, Dragging and Dropping to work.
                            IgnoreZoomLevel = true      // Required as ieDriver will otherwise only function at 100% zoom.
                        };
                        _driver = new InternetExplorerDriver(ApplicationEnvironment.ApplicationBasePath, options);
                        break;
                    }
                case Browser.PhantomJS:
                    {
                        _driver = new PhantomJSDriver(ApplicationEnvironment.ApplicationBasePath);
                        break;
                    }
                default:
                    {
                        _driver = null;
                        throw new ArgumentOutOfRangeException($"No entry matching '{browser}' found in Enum 'Browser'");
                    }
            }

            Navigate = new SeleniumNavigate(_driver);
            Find = new SeleniumFind(_driver);
            Verify = new SeleniumVerify();
            Interact = new SeleniumInteract(_driver);
            Log = new SeleniumLogger(testName, NLog.LogLevel.Info, DateTime.Now, _driver);
        }

        public IWebDriver Driver()
        {
            return _driver;
        }

        public void Kill()
        {
            _driver.Quit();
        }
    }
}
