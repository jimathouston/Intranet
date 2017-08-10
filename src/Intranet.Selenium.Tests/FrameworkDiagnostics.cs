using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using OpenQA.Selenium;
using Intranet.Selenium.Framework;
using System.Linq;

namespace Intranet.Selenium.Tests
{
#pragma warning disable S3881 //full implementation of IDisposable not necessary; only used as a workaround (since xUnit does not support [TearDown] or similar)
    public class FrameworkDiagnostics : IDisposable
    {
        const string Url = "http://34.248.135.81";
        SeleniumDriver driver;
        ResultTracker result;

        [Fact]
        public void LoginTest_Pass() // demo - should pass
        {
            driver = new SeleniumDriver(Browser.Chrome, nameof(LoginTest_Pass));
            result = new ResultTracker(AssertLevel.Soft, driver.Log);
            driver.Log.Write("Test Initiated");

            driver.Navigate.GoToUrl(Url);
            try
            {
                Assert.True(driver.Verify.ElementExists(By.XPath("//form[@class = 'login-form']"), "Login Form"));
                result.Pass();
            }
            catch (Xunit.Sdk.TrueException e)
            {
                result.Fail(e);
            }

            Element usernameInput =
                driver.Find.Elements(By.Name("username"), "Username Input Field");
            driver.Interact.SendKeys(usernameInput, "aUser");

            Element passwordInput =
                driver.Find.Elements(By.Name("password"), "Password Input Field");
            driver.Interact.SendKeys(passwordInput, "aPassword");

            Element loginButton =
                driver.Find.Elements(By.XPath("//form[@class = 'login-form']/button"), "Login Button");
            driver.Interact.Click(loginButton);

            driver.Log.Write("Test Completed");
        }

        [Fact]
        public void LoginTest_Fail() // demo - should fail
        {
            driver = new SeleniumDriver(Browser.Chrome, nameof(LoginTest_Fail));
            result = new ResultTracker(AssertLevel.Soft, driver.Log);
            driver.Log.Write("Test Initiated");

            driver.Navigate.GoToUrl(Url);
            try
            {
                Assert.True(driver.Verify.ElementExists(By.XPath("gibberish"), "Login Form (bad XPath)"));
                result.Pass();
            }
            catch (Xunit.Sdk.TrueException e)
            {
                result.Fail(e);
            }

            Element usernameInput =
                driver.Find.Elements(By.Name("burk"), "Username Input Field (bad identifier)");
            driver.Interact.SendKeys(usernameInput, "aUser");

            Element loginButton =
                driver.Find.Elements(By.XPath("//form[@class = 'login-form']/button"), "Login Button");
            driver.Interact.SendKeys(loginButton, "text to a button");

            driver.Log.Write("Test Completed");
        }

        public void Dispose()
        {
            driver.Kill();
            result.Evaluate();
        }
    }
}
