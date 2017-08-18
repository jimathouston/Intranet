using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Intranet.Selenium.Framework.Enums;

namespace Intranet.Selenium.Framework
{
    public static class Verify
    {
        #region Elements
        /// <summary>
        /// Returns true if at least one IWebElement on the current page matches the provided identifier
        /// </summary>
        /// <param name="identifier">identifier describing the element(s) to find</param>
        /// <returns>True if at least one element matches identifier, otherwise false</returns>
        #pragma warning disable S1481 // Assigning to unused variable necessary to carry out intended function ot method
        public static void ElementExists(By identifier, string name, ISeleniumDriver driver)
        {
            driver.Log($"VERIFY: {name} exists");
            try
            {
                IWebElement foundElements = driver.DriverComponent.FindElement(identifier);
                driver.Log($"{name} is present");
                driver.TestOutcome.PassStep();
            }
            catch (NoSuchElementException)
            {
                driver.Log($"No {name} found");
                driver.TestOutcome.FailStep();
            }
        }
        #endregion

        #region Page
        public static void PageTitleIs(string expected, ISeleniumDriver driver)
        {
            driver.Log($"VERIFY: Current Page Title should be '{expected}'");
            driver.Log($"Page title is: '{driver.DriverComponent.Title}'");
            if (driver.DriverComponent.Title == expected)
            {
                driver.TestOutcome.PassStep();
            }
            else driver.TestOutcome.FailStep();
        }

        public static void PageTitleIsNot(string expected, ISeleniumDriver driver)
        {
            driver.Log($"VERIFY: Current Page Title should NOT be '{expected}'");
            driver.Log($"Page title is: '{driver.DriverComponent.Title}'");
            if (driver.DriverComponent.Title != expected)
            {
                driver.TestOutcome.PassStep();
            }
            else driver.TestOutcome.FailStep();
        }

        public static void PageTitleContains(string expected, ISeleniumDriver driver)
        {
            driver.Log($"VERIFY: Current Page Title should contain '{expected}'");
            driver.Log($"Page title is: '{driver.DriverComponent.Title}'");
            if (driver.DriverComponent.Title.Contains(expected))
            {
                driver.TestOutcome.PassStep();
            }
            else driver.TestOutcome.FailStep();
        }

        public static void PageTitleDoesNotContain(string expected, ISeleniumDriver driver)
        {
            driver.Log($"VERIFY: Current Page Title should NOT contain '{expected}'");
            driver.Log($"Page title is: '{driver.DriverComponent.Title}'");
            if (!driver.DriverComponent.Title.Contains(expected))
            {
                driver.TestOutcome.PassStep();
            }
            else driver.TestOutcome.FailStep();
        }
        #endregion

        #region Basics
        public static void AreEqual(string expected, string expectedName, string actual, string actualName, ISeleniumDriver driver)
        {
            driver.Log($"VERIFY: {actualName} is equal to {expectedName}");
            driver.Log($"Expected: {expected} | Actual: {actual}");
            if (actual == expected)
            {
                driver.TestOutcome.PassStep();
            }
            else driver.TestOutcome.FailStep();
        }

        public static void AreNotEqual(string expected, string expectedName, string actual, string actualName, ISeleniumDriver driver)
        {
            driver.Log($"VERIFY: {actualName} is Not equal to {expectedName}");
            driver.Log($"{expectedName}: {expected} | {actualName}: {actual}");
            if (actual != expected)
            {
                driver.TestOutcome.PassStep();
            }
            else driver.TestOutcome.FailStep();
        }

        public static void Contains(string expected, string expectedName, string actual, string actualName, ISeleniumDriver driver)
        {
            driver.Log($"VERIFY: {actualName} should contain {expectedName}");
            driver.Log($"{expectedName}: {expected} | {actualName}: {actual}");
            if (actual.Contains(expected))
            {
                driver.TestOutcome.PassStep();
            }
            else driver.TestOutcome.FailStep();
        }

        public static void DoesNotContain(string expected, string expectedName, string actual, string actualName, ISeleniumDriver driver)
        {
            driver.Log($"VERIFY: {actualName} should not contain {expectedName}");
            driver.Log($"{expectedName}: {expected} | {actualName}: {actual}");
            if (!actual.Contains(expected))
            {
                driver.TestOutcome.PassStep();
            }
            else driver.TestOutcome.FailStep();
        }
        #endregion

    }
}
