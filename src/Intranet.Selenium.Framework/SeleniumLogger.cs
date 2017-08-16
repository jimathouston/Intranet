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
    public class SeleniumLogger : ISeleniumLogger
    {
        private readonly LogFactory _logFactory;
        private readonly LoggingConfiguration _logConfig;
        private readonly Logger _logger;

        private readonly DateTime _logStart;
        private readonly string _basePath;
        private readonly string _logName;

        public SeleniumLogger(string logName, Level minLevel)
        {
            _logName = logName;
            _logStart = DateTime.Now;
            _basePath = Path.GetFullPath($@"{ApplicationEnvironment.ApplicationBasePath}\..\..\..");
            CreateReportDirectory();

            _logFactory = new LogFactory(new LoggingConfiguration());
            _logConfig = new LoggingConfiguration();
            _logger = GetLogger(_logName, GetNlogLevel(minLevel));

        }

        /// <summary>
        /// Write a message to the log, using default level (info)
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Write(string message)
        {
            Write(message, Level.Info);
        }

        /// <summary>
        /// Write a message to the log, using the specified level
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="level">Level of message</param>
        public void Write(string message, Level level)
        {
            switch (level)
            {
                case Level.Trace:
                    _logger.Trace(message);
                    break;
                case Level.Debug:
                    _logger.Debug(message);
                    break;
                case Level.Info:
                    _logger.Info(message);
                    break;
                case Level.Warn:
                    _logger.Warn(message);
                    break;
                case Level.Error:
                    _logger.Error(message);
                    break;
                case Level.Fatal:
                    _logger.Fatal(message);
                    break;
                default:
                    _logger.Info(message);
                    break;
            }
        }

        /// <summary>
        /// Save a screenshot of the current browser view
        /// </summary>
        /// <param name="fileName">Name of screenshot</param>
        public void SaveScreenshot(string fileName, ISeleniumDriver driver)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver.DriverComponent).GetScreenshot();
            string savePath = GetSavePath();
            string time = _logStart.ToString("yyyyMMdd-HHmm");

            string fullPath = $@"{savePath}\{time}_{_logName}_{fileName}.png";

            Write($"Saving Screenshot as: {fullPath}", Level.Info);
            screenshot.SaveAsFile(fullPath, ScreenshotImageFormat.Png);
        }

        /// <summary>
        /// Instantiate a Logger with the specified name and minimum message level
        /// </summary>
        /// <param name="name">Name of logger</param>
        /// <param name="minLevel">Messages of lower level than this will not be logged</param>
        /// <returns>Logger with appropriate name, minimum level and settings</returns>
        private Logger GetLogger(string name, NLog.LogLevel minLevel)
        {
            string savePath = GetSavePath();                                    //Generate save path 

            var fileTarget = new FileTarget();                                  // Create a target
            var consoleTarget = new ConsoleTarget();

            if (String.IsNullOrWhiteSpace(name))
            {
                name = "Log";
            }

            _logConfig.AddTarget($"{name}_file", fileTarget);                   // Add the target to the configuration
            _logConfig.AddTarget($"{name}_console", consoleTarget);

            string time = _logStart.ToString("yyyyMMdd-HHmm");

            fileTarget.FileName = $@"{savePath}\{time}_{name}.log";             //set name (and path) of savefile
            fileTarget.Layout = @"${date} | ${level} | ${message}";             //Define layout for file

            consoleTarget.Layout = @"${time} | ${level} | ${message}";

            var fileRule = new LoggingRule(name, minLevel, fileTarget);         //Create logging rules...
            var consoleRule = new LoggingRule(name, minLevel, consoleTarget);

            _logConfig.LoggingRules.Add(fileRule);                              //...and add to configuration
            _logConfig.LoggingRules.Add(consoleRule);

            _logFactory.Configuration = _logConfig;                              //Activate the configuration

            return _logFactory.GetLogger(name);

        }

        /// <summary>
        /// Create the directory where logs will be saved (if it does not already exist)
        /// </summary>
        private void CreateReportDirectory()
        {
            string savePath = GetSavePath();
            Directory.CreateDirectory(savePath);
        }

        /// <summary>
        /// Calculate and return savepath for logs and screenshots.
        /// </summary>
        /// <returns>Savepath as string</returns>
        private string GetSavePath()
        {
            string compiledPath = $@"{_basePath}\log";

            return compiledPath;
        }

        private NLog.LogLevel GetNlogLevel(Level level)
        {
            switch (level)
            {
                case Level.Trace:
                    return NLog.LogLevel.Trace;
                case Level.Debug:
                    return NLog.LogLevel.Debug;
                case Level.Info:
                    return NLog.LogLevel.Info;
                case Level.Warn:
                    return NLog.LogLevel.Warn;
                case Level.Error:
                    return NLog.LogLevel.Error;
                case Level.Fatal:
                    return NLog.LogLevel.Fatal;
                default:
                    return NLog.LogLevel.Info;
            }
        }
    }
}
