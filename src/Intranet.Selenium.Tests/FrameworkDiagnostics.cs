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
        const string Url = "http://34.248.135.81";

        [Fact]
        public void ATest()
        {
            SeleniumDriver driver = new SeleniumDriver(Browser.Chrome, nameof(ATest));
            driver.Log.Write($"Test Initiated: {DateTime.Now}");

            driver.Navigate.GoToUrl(Url);
            driver.Log.SaveScreenshot("Navigation");
            driver.Log.Write("Navigation Successful");

            driver.Kill();
            driver.Log.Write($"Test completed: {DateTime.Now}");
        }

        [Fact]
        public void BTest()
        {
            SeleniumDriver driver = new SeleniumDriver(Browser.InternetExplorer, nameof(BTest));
            driver.Log.Write($"Test Initiated");

            driver.Navigate.GoToUrl(Url);
            driver.Log.SaveScreenshot("Navigation");
            driver.Log.Write("Navigation Successful");

            driver.Kill();
            driver.Log.Write($"Test completed");
        }
    }
}
