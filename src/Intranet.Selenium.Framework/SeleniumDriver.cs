using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
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

        public SeleniumDriver (Browser browser)
        {
            switch (browser)
            {
                case Browser.Chrome:
                    {
                        Driver = new ChromeDriver();
                        break;
                    }
                case Browser.Firefox:
                    {
                        Driver = new FirefoxDriver();
                        break;
                    }
                case Browser.InternetExplorer:
                    {
                        InternetExplorerOptions options = new InternetExplorerOptions()
                        {
                            EnableNativeEvents = true,  // Required for Clicking, Dragging and Dropping to work.
                            RequireWindowFocus = true,  // Required for Clicking, Dragging and Dropping to work.
                            IgnoreZoomLevel = true      // Required as ieDriver will otherwise only function at 100% zoom.
                        };
                        Driver = new InternetExplorerDriver(options);
                        break;
                    }
                case Browser.PhantomJS:
                    {
                        Driver = new PhantomJSDriver();
                        break;
                    }
                default:
                    {
                        Driver = null;
                        throw new ArgumentOutOfRangeException($"No entry matching '{browser}' found in Enum 'Browser'");
                    }
            }
        }

        public void Close()
        {
            Driver.Quit();
        }

        public SeleniumNavigate Navigate()
        {
            return new SeleniumNavigate(Driver);
        }

        public SeleniumFind Find()
        {
            return new SeleniumFind(Driver);
        }
    }
}
