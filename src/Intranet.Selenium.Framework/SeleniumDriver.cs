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
        public IWebDriver Driver { get; }
        public SeleniumNavigate Navigate { get; }
        public SeleniumFind Find { get; }
        public SeleniumVerify Verify { get; }
        public SeleniumLogger Log { get; }

        public SeleniumDriver (Browser browser)
        {
            switch (browser)
            {
                case Browser.Chrome:
                    {
                        Driver = new ChromeDriver(ApplicationEnvironment.ApplicationBasePath);
                        break;
                    }
                case Browser.Firefox:
                    {
                        Driver = null;
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
                        Driver = new InternetExplorerDriver(ApplicationEnvironment.ApplicationBasePath, options);
                        break;
                    }
                case Browser.PhantomJS:
                    {
                        Driver = new PhantomJSDriver(ApplicationEnvironment.ApplicationBasePath);
                        break;
                    }
                default:
                    {
                        Driver = null;
                        throw new ArgumentOutOfRangeException($"No entry matching '{browser}' found in Enum 'Browser'");
                    }
            }

            Navigate = new SeleniumNavigate(Driver);
            Find = new SeleniumFind(Driver);
            Verify = new SeleniumVerify();
            Log = new SeleniumLogger("", NLog.LogLevel.Info, DateTime.Now, Driver);
        }

        public void Kill()
        {
            Driver.Quit();
        }
    }
}
