using System;
using System.Collections.Generic;
using System.Text;
using Intranet.Selenium.Framework.Enums;
using Intranet.Selenium.Framework;
using Xunit;

namespace Intranet.Selenium.Framework
{
    public class ResultTrackerXUnit : IResultTracker
    {
        private readonly AssertLevel _assertLevel;
        private readonly ISeleniumLogger _logger;

        private bool _hasFailed;
        private bool _hasPassed;

        public ResultTrackerXUnit(AssertLevel assertLevel, ISeleniumLogger logger)
        {
            _assertLevel = assertLevel;
            _logger = logger;
            _hasFailed = false;
            _hasPassed = false;
        }

        public void PassStep()
        {
            _hasPassed = true;
            _logger.Write("PASS");
        }

        public void FailStep()
        {
            _hasFailed = true;
            _logger.Write("FAIL", Level.Error);
            if (_assertLevel == AssertLevel.Hard)
            {
                Assert.True(false, "Hard Assert - FAIL");
            }
        }

        public void Evaluate()
        {
            try
            {
                Assert.False(_hasFailed);
                try
                {
                    Assert.True(_hasPassed);
                    _logger.Write("TEST RESULT: PASS");
                }
                catch (Xunit.Sdk.TrueException)
                {
                    _logger.Write("TEST RESULT: INCONCLUSIVE", Level.Warn);
                    throw;
                }
            }
            catch(Xunit.Sdk.FalseException)
            {
                _logger.Write("TEST RESULT: FAIL", Level.Fatal);
                throw;
            }
        }
    }
}
