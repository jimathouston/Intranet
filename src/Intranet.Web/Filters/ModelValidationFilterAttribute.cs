using Intranet.Web.Models.Validation;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Web.Filters
{
    /// <summary>
    /// If ModelState is invalid the filter returns the ModelState and HTTP status code 422 Unprocessable Entity
    /// Based on http://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi/
    /// </summary>
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}
