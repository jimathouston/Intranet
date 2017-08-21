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
        const string testExtraTag = "BiggerLongerAndUncut";

        [Theory]
        [InlineData(AccountLevel.Admin)]
        [InlineData(AccountLevel.User)]
        public void AllUsersCanCrudOwnNewsItems(AccountLevel account)
        {
            string pTitle = $"{testTitle} {DateTime.Now.ToString()}";
            string pText = testText;
            string pTags = testTags;
            string pExtraTag = testExtraTag;

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

                // *** CREATE ***

                CreateAndPostNewsItem(driver, pTitle, pText, pTags);

                // *** READ ***

                // return to start page
                Navigate.GoToUrl(testUrl, driver);

                // verify the news item is present

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

                Interact.SendKeys(updateTags, $", {pExtraTag}", driver);

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

                Verify.Contains(pExtraTag, "Tag added in Update operation", UpdatedItemTags.FirstOrDefault().Text, "Tags of Selected News Item", driver);

                // *** DELETE ***

                // Find and Click Delete Button
                Element btnDelete = Find.Elements(
                    By.XPath("//div[@class = 'card-action']/a[text() = 'Delete']"),
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

        [Theory]
        [InlineData(AccountLevel.Admin, AccountLevel.User, false)]
        [InlineData(AccountLevel.User, AccountLevel.Admin, true)]
        public void OnlyAdminCanCrudOtherUsersItem(AccountLevel userA, AccountLevel userB, bool shouldEditDelete)
        {
            string pTitle = $"{testTitle} {DateTime.Now.ToString()}";
            string pText = testText;
            string pTags = testTags;
            string pExtraTag = testExtraTag;

            ISeleniumDriver driver = new SeleniumDriver(
                Browser.Chrome,
                AssertLevel.Soft,
                new SeleniumLogger($"{nameof(OnlyAdminCanCrudOtherUsersItem)}_{userB}", Level.Info));

            try
            {
                // Login userA
                Navigate.GoToUrl(testUrl, driver);
                LoginUtil.Login(userA, driver);

                // Create Article
                CreateAndPostNewsItem(driver, pTitle, pText, pTags);

                // Logout userA
                LoginUtil.LogOut(driver);

                // Login userB
                LoginUtil.Login(userB, driver);

                // Read Article
                SelectFirstnewsItemAndEvaluateTitle(driver, pTitle);

                // Edit Article
                Element btnUpdateItem = Find.Elements(
                    By.XPath("//div[@class = 'card-action']/a[text() = 'Edit']"),
                    "Update Button",
                    timeOut: 10000,
                    driver: driver);

                if ((btnUpdateItem.FirstOrDefault() != null) == shouldEditDelete)
                {
                    driver.TestOutcome.PassStep();
                }
                else driver.TestOutcome.FailStep();

                if (shouldEditDelete)
                {
                    Interact.Click(btnUpdateItem, driver);

                    Element updateTags = Find.Elements(
                        By.Id("Tags"),
                        "Tags Input Field",
                        driver: driver);

                    Interact.SendKeys(updateTags, $", {pExtraTag}", driver);

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

                    Verify.Contains(pExtraTag, "Tag added in Update operation", UpdatedItemTags.FirstOrDefault().Text, "Tags of Selected News Item", driver);
                }

                // Delete Article
                // Find and Click Delete Button
                Element btnDelete = Find.Elements(
                    By.XPath("//div[@class = 'card-action']/a[text() = 'Delete']"),
                    "Delete Item button",
                    driver);
                if ((btnUpdateItem.FirstOrDefault() != null) == shouldEditDelete)
                {
                    driver.TestOutcome.PassStep();
                }
                else driver.TestOutcome.FailStep();

                if (shouldEditDelete)
                {
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

        private void CreateAndPostNewsItem(ISeleniumDriver driver, string pTitle, string pText, string pTags)
        {
            // Find and click the "Create New" button
            Element btnCreateNew = Find.Elements(
                By.XPath("//main/div[@class = 'container']//a[@class = 'btn']"),
                "Create New-button",
                timeOut: 10000,
                driver: driver);

            Interact.Click(btnCreateNew, driver);

            // Find all required fields & Input values
            Element inputTitle = Find.Elements(
                By.Id("Title"),
                "Title Input Field",
                timeOut: 10000,
                driver: driver);
            Interact.SendKeys(inputTitle, pTitle, driver);

            Element inputText = Find.Elements(
                By.Id("Text_ifr"),
                "Text Input Field",
                driver: driver);
            Interact.SendKeys(inputText, " ", driver); // Just to get its attention
            Interact.SendKeys(inputText, pText, driver);

            Element inputTags = Find.Elements(
                By.Id("Tags"),
                "Tags Input Field",
                driver: driver);
            Interact.SendKeys(inputTags, pTags, driver);

            // Click the publish toggle, then the create button.

            Element togglePublished = Find.Elements(
                By.XPath("//span[@class = 'lever']"),
                "Publish-toggle",
                driver: driver);
            Interact.Click(togglePublished, driver);

            Element btnSubmit = Find.Elements(
                By.XPath("//div[@class = 'input-field']/input[@class = 'btn']"),
                "Create Button",
                driver: driver);
            Interact.Click(btnSubmit, driver);
        }
 
    }
}
