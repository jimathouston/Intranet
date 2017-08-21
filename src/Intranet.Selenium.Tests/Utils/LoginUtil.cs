using Intranet.Selenium.Framework;
using Intranet.Selenium.Framework.Enums;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Intranet.Selenium.Tests.Utils
{
    public static class LoginUtil
    {
        private const string _successfulLoginTitle = "News - Certaincy Intranet";
        private static readonly KeyValuePair<string, string> _adminCredentials = new KeyValuePair<string, string>("admin", "password1");
        private static readonly KeyValuePair<string, string> _userCredentials = new KeyValuePair<string, string>("test", "password1");

        public static void Login(AccountLevel account, ISeleniumDriver driver)
        {
            KeyValuePair<string, string> loginCredentials;

            switch (account)
            {
                case AccountLevel.Admin:
                    loginCredentials = _adminCredentials;
                    break;
                case AccountLevel.User:
                    loginCredentials = _userCredentials;
                    break;
            }
            driver.Log($"Attempting Login as {account}");
            try
            {
                PerformLogin(loginCredentials, driver);
                driver.Log("Login Successful");
            }
            catch (Exception e)
            {
                string exception = e.GetType().ToString();
                driver.Log($"Login Failed: {exception}", Level.Fatal);
                throw;
            }
        }

        public static void LogOut(ISeleniumDriver driver)
        {
            Element btnShowMenu = Find.Elements(
                By.XPath("//a[@class = 'button-collapse']"),
                "Show Menu Button",
                driver);

            if (btnShowMenu.FirstOrDefault().Displayed)
            {
                Interact.Click(btnShowMenu, driver);
            }

            Element btnLogOutTop = Find.Elements(
                By.XPath("//a[contains (@href, 'Logout')]"),
                "Logout Button",
                driver);

            foreach (IWebElement element in btnLogOutTop.Elements)
            {
                if (element.Displayed)
                {
                    Interact.Click(element, btnLogOutTop.Name, driver);
                    if (driver.DriverComponent.Url.Contains("Login"))
                    {
                        break;
                    }
                }
            }
            if (driver.DriverComponent.Url.Contains("Login"))
            {
                driver.Log("Logout Successful");
            }
            else
            {
                driver.Log("Logout Failed - see log for details.", Level.Warn);
            }

        }

        private static void PerformLogin(KeyValuePair<string, string> loginCredentials, ISeleniumDriver driver)
        {
            Element usernameInput =
                Find.Elements(By.Name("username"), "Username Input Field", driver);
            Interact.SendKeys(usernameInput, loginCredentials.Key, driver);

            Element passwordInput =
                Find.Elements(By.Name("password"), "Password Input Field", driver);
            Interact.SendKeys(passwordInput, loginCredentials.Value, driver);

            Element loginButton =
                Find.Elements(By.XPath("//form[@class = 'login-form']/button"), "Login Button", driver);
            Interact.Click(loginButton, driver);

            Verify.PageTitleIs(_successfulLoginTitle, driver);
        }
    }
}
