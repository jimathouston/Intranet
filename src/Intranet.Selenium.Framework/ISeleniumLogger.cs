using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DotNet.PlatformAbstractions;
using System.IO;
using OpenQA.Selenium;
using Intranet.Selenium.Framework.Enums;

namespace Intranet.Selenium.Framework
{
    public interface ISeleniumLogger
    {
        /// <summary>
        /// Write a message to the log, using default level (info)
        /// </summary>
        /// <param name="message">Message to log</param>
        void Write(string message);

        /// <summary>
        /// Write a message to the log, using the specified level
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="level">Level of message</param>
        void Write(string message, Level level);

        /// <summary>
        /// Save a screenshot of the current browser view
        /// </summary>
        /// <param name="fileName">Name of screenshot</param>
        void SaveScreenshot(string fileName, ISeleniumDriver driver);
    }
}
