using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Intranet.Web.Factories;
using Intranet.Web.Models;
using Intranet.Web.Provider;
using Xunit;

namespace Intranet.Web.UnitTests.Factories
{
  public class DateTimeFactory_Fact
  {
    [Fact]
    public void Return_Should_Be_DateTime()
    {
      // Assign
      var dateTimeFactory = new DateTimeFactory();

      // Act
      var dateTime = dateTimeFactory.GetCurrentDateTime();

      // Assert
      Assert.IsType<DateTime>(dateTime);
    }

    [Fact]
    public void Return_Should_Be_DateTimeOffset()
    {
      // Assign
      var dateTimeFactory = new DateTimeFactory();

      // Act
      var dateTime = dateTimeFactory.GetCurrentDateTimeOffset();

      // Assert
      Assert.IsType<DateTimeOffset>(dateTime);
    }
  }
}
