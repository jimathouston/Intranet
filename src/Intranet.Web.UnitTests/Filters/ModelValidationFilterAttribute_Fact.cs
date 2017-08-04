using Intranet.Web.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Intranet.Web.Models;
using Intranet.Web.Models.Validation;

namespace Intranet.Web.UnitTests.Filters
{
    public class ModelValidationFilterAttribute_Fact
    {
        [Fact]
        public void PassWhenModelStateIsValid()
        {
            // Arrange
            var modelValidationFilterAttribute = new ModelValidationFilterAttribute();
            var modelState = new ModelStateDictionary();

            var actionContext = new ActionContext(
               new Mock<HttpContext>().Object,
               new Mock<RouteData>().Object,
               new Mock<ActionDescriptor>().Object,
               modelState
            );

            var actionExcecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>()
            );

            // Act
            modelValidationFilterAttribute.OnActionExecuting(actionExcecutingContext);

            // Assert
            Assert.Equal(actionExcecutingContext.Result, null);
        }

        [Fact]
        public void FailWhenModelStateIsInvalid()
        {
            // Arrange
            var modelValidationFilterAttribute = new ModelValidationFilterAttribute();
            var modelState = new ModelStateDictionary();

            var actionContext = new ActionContext(
               new Mock<HttpContext>().Object,
               new Mock<RouteData>().Object,
               new Mock<ActionDescriptor>().Object,
               modelState
            );

            var actionExcecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>()
            );

            modelState.AddModelError("name", "invalid");

            // Act
            modelValidationFilterAttribute.OnActionExecuting(actionExcecutingContext);

            // Assert
            Assert.Equal(((ValidationFailedResult)actionExcecutingContext.Result).StatusCode, 422);
        }
    }
}
