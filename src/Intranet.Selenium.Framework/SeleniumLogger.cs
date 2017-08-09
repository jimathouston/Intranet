using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DotNet.PlatformAbstractions;
using System.IO;
using OpenQA.Selenium;

namespace Intranet.Selenium.Framework
{
    public enum Level
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    public class SeleniumLogger
    {
        private readonly LoggingConfiguration _logConfig;
        private readonly Logger _logger;

        private readonly ITakesScreenshot _screenshotDriver;

        private readonly DateTime _logStart;
        private readonly string _basePath;
        private readonly string _logName;

        public SeleniumLogger(string logName, NLog.LogLevel minLevel, DateTime logStart, IWebDriver driver)
        {
            _logName = logName;
            _logStart = logStart;
            _basePath = ApplicationEnvironment.ApplicationBasePath;
            CreateReportDirectory();

            _logConfig = new LoggingConfiguration();
            _logger = GetLogger(logName, minLevel);

            _screenshotDriver = driver as ITakesScreenshot;

        }

        public void Write(string message)
        {
            Write(message, Level.Info);
        }

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

        public void SaveScreenshot(string fileName)
        {
            Screenshot screenshot = _screenshotDriver.GetScreenshot();
            string savePath = GetSavePath();
            string fullPath = $@"{savePath}\{fileName}.png";

            Log($"Saving Screenshot as: {fullPath}", Level.Info);
            screenshot.SaveAsFile(fullPath, ScreenshotImageFormat.Png);
        }

        private Logger GetLogger(string name, NLog.LogLevel minLevel)
        {
            string savePath = GetSavePath();                                    //Generate save path 

            var fileTarget = new FileTarget();                                  // Create a target
            var consoleTarget = new ConsoleTarget();

            _logConfig.AddTarget($"{name}_file", fileTarget);                   // Add the target to the configuration
            _logConfig.AddTarget($"{name}_console", consoleTarget);

            if (String.IsNullOrWhiteSpace(name))
            {
                name = "Log";
            }

            string time = _logStart.ToString("yyyyMMdd-HHmm");

            fileTarget.FileName = $@"{savePath}\{time}_{name}.log";                    //set name (and path) of savefile
            fileTarget.Layout = @"${date}:${time} | ${level} | ${message}";     //Define layout for file

            consoleTarget.Layout = @"${time} | ${level} | ${message}";

            var fileRule = new LoggingRule(name, minLevel, fileTarget);         //Create logging rules...
            var consoleRule = new LoggingRule(name, minLevel, consoleTarget);

            _logConfig.LoggingRules.Add(fileRule);                              //...and add to configuration
            _logConfig.LoggingRules.Add(consoleRule);

            LogManager.Configuration = _logConfig;                              //Activate the configuration

            return LogManager.GetLogger(name);

        }

        private void CreateReportDirectory()
        {
            string savePath = GetSavePath();
            Directory.CreateDirectory(savePath);
        }

        private string GetSavePath()
        {
            string compiledPath = $@"{_basePath}\Logs\{_logName}";

            return compiledPath;
        }
    }
}
