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
        const string testTitle = "RE: King Torg";
        const string testText = "All Hail King Torg!";
        const string testTags = "Test, King Torg";

        [Theory]
        [InlineData(AccountLevel.Admin)]
        [InlineData(AccountLevel.User)]
        public void AllUsersCanCrudOwnNewsItems(AccountLevel account)
        {
            string pTitle = $"{testTitle}_{DateTime.Now.ToString()}";
            string pText = testText;
            string pTags = testTags;
            string pUpdatedTags = ", BiggerLongerAndUncut";

            // Setup
            ISeleniumDriver driver = new SeleniumDriver(
                Browser.Chrome,
                AssertLevel.Soft,
                new SeleniumLogger($"{nameof(AllUsersCanCrudOwnNewsItems)}_{account}", Level.Info));

            try
            {
                // Go to URL
                Navigate.GoToUrl(testUrl, driver);

                // Login with appropriate account
                LoginUtil.Login(account, driver);

                Verify.PageTitleIs("News - Certaincy Intranet", driver);

                // *** CREATE ***

                // Find and click the "Create New" button
                Element btnCreateNew = Find.Elements(
                    By.XPath("//main/div[@class = 'container']//a[@class = 'btn']"),
                    "Create New-button",
                    timeOut: 10000,
                    driver: driver);

                Interact.Click(btnCreateNew, driver);

                // Find all required elements
                Element inputTitle = Find.Elements(
                    By.Id("Title"),
                    "Title Input Field",
                    timeOut: 10000,
                    driver: driver);

                Element inputText = Find.Elements(
                    By.Id("Text"),
                    "Text Input Field",
                    driver: driver);

                Element inputTags = Find.Elements(
                    By.Id("Tags"),
                    "Tags Input Field",
                    driver: driver);

                Element togglePublished = Find.Elements(
                    By.Id("Published"),
                    "Publish-toggle",
                    driver: driver);

                Element btnSubmit = Find.Elements(
                    By.XPath("//div[@class = 'input-field']/input[@class = 'btn']"),
                    "Create Button",
                    driver: driver);

                // Input Values
                Interact.SendKeys(inputTitle, pTitle, driver);
                Interact.SendKeys(inputText, pText, driver);
                Interact.SendKeys(inputTags, pTags, driver);
                Interact.Click(togglePublished, driver);
                Interact.Click(btnSubmit, driver);

                // *** READ ***

                // return to start page
                Navigate.GoToUrl(testUrl, driver);

                // verify newly posted news item is visible

                SelectFirstnewsItemAndEvaluateTitle(driver, pTitle);

                // Verify remaining attributes

                Element ItemText = Find.Elements(
                    By.XPath("//div[@class = 'card-content']/p[3]"),
                    "News Item Text",
                    timeOut: 10000,
                    driver: driver);

                Verify.AreEqual(pText, "Text of created News Item", ItemText.FirstOrDefault().Text, "Text of Selected News Item", driver);

                Element ItemTags = Find.Elements(
                    By.XPath("//p[@class = 'card-tags']/em"),
                    "News Item Tags",
                    driver);

                Verify.AreEqual(pTags, "Tags of created News Item", ItemTags.FirstOrDefault().Text, "Tags of Selected News Item", driver);

                // *** UPDATE ***

                // Go to news Page
                Navigate.GoToUrl(testUrl, driver);

                // Click the top news item, and verify it is the same that we posted before
                SelectFirstnewsItemAndEvaluateTitle(driver, pTitle);

                // Click the Edit Button and change the tags

                Element btnUpdateItem = Find.Elements(
                    By.XPath("//div[@class = 'card-action']/a[text() = 'Edit']"),
                    "Update Button",
                    timeOut: 10000,
                    driver: driver);

                Interact.Click(btnUpdateItem, driver);

                Element updateTags = Find.Elements(
                    By.Id("Tags"),
                    "Tags Input Field",
                    driver: driver);

                Interact.SendKeys(updateTags, pUpdatedTags, driver);

                Element btnSubmitUpdate = Find.Elements(
                    By.XPath("//div[@class = 'input-field']/input[@class = 'btn']"),
                    "Create Button",
                    driver: driver);

                Interact.Click(btnSubmitUpdate, driver);

                // Go to news Page
                Navigate.GoToUrl(testUrl, driver);

                // Click the top news item, and verify it is the same that we posted before
                SelectFirstnewsItemAndEvaluateTitle(driver, pTitle);

                // Verify that the tags have been updated
                Element UpdatedItemTags = Find.Elements(
                    By.XPath("//p[@class = 'card-tags']/em"),
                    "News Item Tags",
                    driver);

                Verify.AreEqual($"{pTags}{pUpdatedTags}", "Tags of created News Item", UpdatedItemTags.FirstOrDefault().Text, "Tags of Selected News Item", driver);

                // *** DELETE ***

                // Find and Click Delete Button
                Element btnDelete = Find.Elements(
                    By.XPath("//div[@class = 'card-action']/a[text() = 'Delete"),
                    "Delete Item button",
                    driver);

                Interact.Click(btnDelete, driver);

                // Click Confirmation Button
                Element btnConfirmDelete = Find.Elements(
                    By.XPath("//input[@value = 'Delete']"),
                    "Confirm Deletion Button",
                    timeOut: 10000,
                    driver: driver);

                Interact.Click(btnConfirmDelete, driver);

                // Go to news page
                Navigate.GoToUrl(testUrl, driver);

                // Confirm news Item is no Longer present
                driver.Log("VERIFY: Deletion of News Item");
                Element allNewsItems = Find.Elements(
                    By.XPath("//div[@class = 'container-fluid']/div[@class = 'row']"),
                    "All Currently Displayed News Items",
                    timeOut: 10000,
                    driver: driver);

                if (allNewsItems.Elements.Count == 0)
                {
                    driver.TestOutcome.PassStep();
                }
                else
                {
                    foreach (IWebElement element in allNewsItems.Elements)
                    {
                        string title = element.FindElement(By.XPath("//span[@class = 'card-title']")).Text;
                        Verify.AreNotEqual(pTitle, "Title of Created News Item", title, "Title of Found News Item", driver);
                    }
                }
            }
            finally
            {
                driver.CloseBrowser();
                driver.TestOutcome.Evaluate();
            }
        }

        private void SelectFirstnewsItemAndEvaluateTitle(ISeleniumDriver driver, string expectedTitle)
        {
            Element firstNewsItem = Find.Elements(
                    By.XPath("(//div[@class = 'container-fluid']/div[@class = 'row'])[1]"),
                    "First News Item",
                    timeOut: 10000,
                    driver: driver);

            string firstItemTitle = firstNewsItem.FirstOrDefault().FindElement(By.XPath("//span[@class = 'card-title']")).Text;

            Verify.AreEqual(expectedTitle, "Title of created News Item", firstItemTitle, "Title of first news item in List", driver);

            Interact.Click(firstNewsItem, driver);
        }
 
    }
}
