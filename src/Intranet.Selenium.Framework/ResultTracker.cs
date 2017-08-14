using System;
using System.Collections.Generic;
using System.Text;
using Intranet.Selenium.Framework.Enums;

namespace Intranet.Selenium.Framework
{
    public class ResultTracker
    {
        private readonly AssertLevel _assertLevel;
        private readonly SeleniumLogger _logger;

        private bool _hasFailed;
        private bool _hasPassed;
        private Exception _firstExceptionThrown;

        public ResultTracker(AssertLevel assertLevel, SeleniumLogger logger)
        {
            _assertLevel = assertLevel;
            _logger = logger;
            _hasFailed = false;
            _hasPassed = false;
        }

        public void Pass()
        {
            _hasPassed = true;
            _logger.Write("PASS");
        }

        public void Fail(Exception e)
        {
            _hasFailed = true;
            _logger.Write("FAIL", Level.Error);

            if (_assertLevel == AssertLevel.Soft)
            {
                if (_firstExceptionThrown == null) _firstExceptionThrown = e;
            }
            else
            {
                throw e;
            }
        }

        public void Evaluate()
        {
            if (_hasFailed)
            {
                _logger.Write("TEST RESULT: FAIL", Level.Fatal);
                throw _firstExceptionThrown;
            }
            else if (_hasPassed)
            {
                _logger.Write("TEST RESULT: PASS");
            }
            else
            {
                _logger.Write("TEST RESULT: INCONCLUSIVE", Level.Warn);
            }
        }
    }
}
