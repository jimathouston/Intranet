using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using OpenQA.Selenium;
using Intranet.Selenium.Framework;
using Intranet.Selenium.Framework.Enums;
using Intranet.Selenium.Tests.Utils;
using System.Linq;

namespace Intranet.Selenium.Tests
{ 
    public class FrameworkDiagnostics
    {
        const string testUrl = "http://localhost:53491";

        [Fact(Skip = "Diagnostics Only")]
        public void LoginTest_Admin()
        {
            ISeleniumDriver driver = new SeleniumDriver(
                Browser.PhantomJS, 
                AssertLevel.Soft, 
                new SeleniumLogger(nameof(LoginTest_Admin), Level.Info));

            try
            {
                Navigate.GoToUrl(testUrl, driver);

                LoginUtil.Login(AccountLevel.Admin, driver);

                driver.Log("Test Completed");
            }
            catch (Exception e)
            {
                driver.Log($"Unhandled Exception: {e.GetType().ToString()}", Level.Fatal);
                driver.Log(e.Message, Level.Fatal);
                driver.Log(e.StackTrace, Level.Fatal);
                driver.TestOutcome.FailStep();
            }
            finally
            {
                driver.CloseBrowser();
                driver.TestOutcome.Evaluate();
            }
        }

        [Fact(Skip = "Diagnostics Only")]
        public void LoginTest_User()
        {
            ISeleniumDriver driver = new SeleniumDriver(
                Browser.PhantomJS,
                AssertLevel.Soft,
                new SeleniumLogger(nameof(LoginTest_User), Level.Info));

            try
            {
                Navigate.GoToUrl(testUrl, driver);

                LoginUtil.Login(AccountLevel.User, driver);

                driver.Log("Test Completed");
            }
            catch (Exception e)
            {
                driver.Log($"Unhandled Exception: {e.GetType().ToString()}", Level.Fatal);
                driver.Log(e.Message, Level.Fatal);
                driver.Log(e.StackTrace, Level.Fatal);
                driver.TestOutcome.FailStep();
            }
            finally
            {
                driver.CloseBrowser();
                driver.TestOutcome.Evaluate();
            }
        }
    }
}
