using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using OpenQA.Selenium;
using Intranet.Selenium.Framework;
using System.Linq;

namespace Intranet.Selenium.Tests
{
    public class FrameworkDiagnostics
    {
        [Fact]
        public void ATest()
        {
            SeleniumDriver driver = new SeleniumDriver(Browser.Chrome);

            driver.Navigate.GoToUrl("www.google.com");

            driver.Kill();
        }

        [Fact]
        public void BTest()
        {
            SeleniumDriver driver = new SeleniumDriver(Browser.InternetExplorer);

            driver.Navigate.GoToUrl("www.google.com");

            driver.Kill();
        }
    }
}
