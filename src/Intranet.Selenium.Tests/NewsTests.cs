using System;
using System.Collections.Generic;
using System.Text;
using Intranet.Selenium.Framework;
using Intranet.Selenium.Framework.Enums;
using Intranet.Selenium.Tests.Utils;
using Xunit;
using OpenQA.Selenium;

namespace Intranet.Selenium.Tests
{
    public class NewsTests
    {
        const string testUrl = "http://localhost:53491";

        [Theory]
        [InlineData(AccountLevel.Admin)]
        [InlineData(AccountLevel.User)]
        public void AllUsersCanCrudOwnNewsItems(AccountLevel account)
        {
            // Setup
            ISeleniumDriver driver = new SeleniumDriver(
                Browser.PhantomJS,
                AssertLevel.Soft,
                new SeleniumLogger($"{nameof(AllUsersCanCrudOwnNewsItems)}_{account}", Level.Info));

            try
            {
                // Go to URL
                Navigate.GoToUrl(testUrl, driver);

                // Login with appropriate account
                LoginUtil.Login(account, driver);

                Verify.PageTitleIs("News - Certaincy Intranet", driver);

                // CREATE

                //todo

                // READ

                // UPDATE

                // DELETE
            }
            finally
            {
                driver.CloseBrowser();
                driver.TestOutcome.Evaluate();
            }
        }
 
    }
}
