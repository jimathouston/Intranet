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
        public void LoginTest_Pass()
        {
            SeleniumDriver driver = new SeleniumDriver(Browser.Chrome, nameof(LoginTest_Pass));
            driver.Log.Write("Test Initiated");

            driver.Navigate.GoToUrl(Url);
            try
            {
                Assert.True(driver.Verify.ElementExists(By.XPath("//form[@class = 'login-form']"), "Login Form"));
                driver.Log.Write("PASS");
            }
            catch (Xunit.Sdk.TrueException)
            {
                driver.Log.Write("FAIL");
                throw;
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
            driver.Kill();
        }

        [Fact]
        public void LoginTest_Fail()
        {
            SeleniumDriver driver = new SeleniumDriver(Browser.Chrome, nameof(LoginTest_Fail));
            driver.Log.Write("Test Initiated");

            driver.Navigate.GoToUrl(Url);
            try
            {
                Assert.True(driver.Verify.ElementExists(By.XPath("gibberish"), "Login Form (bad XPath)"));
                driver.Log.Write("PASS");
            }
            catch (Xunit.Sdk.TrueException)
            {
                driver.Log.Write("FAIL");
            }

            Element usernameInput =
                driver.Find.Elements(By.Name("burk"), "Username Input Field (bad identifier)");
            driver.Interact.SendKeys(usernameInput, "aUser");

            Element loginButton =
                driver.Find.Elements(By.XPath("//form[@class = 'login-form']/button"), "Login Button");
            driver.Interact.SendKeys(loginButton, "text to a button");

            driver.Log.Write("Test Completed");
            driver.Kill();

        }
    }
}
