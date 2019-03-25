using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SerilogTests
{
    public class LoggingActionFilter : ActionFilterAttribute
    {
        private readonly string _argumentName;

        public LoggingActionFilter(string argumentName = null)
        {
            _argumentName = argumentName ?? "quoteId";
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //var result = context.Result;

            //if (result is BadRequestObjectResult badRequest)
            //{
            //    Log.ForContext(context.Controller.GetType()).Warning(
            //        "Bad Request for {Action}: {Phrase}",
            //        context.ActionDescriptor.DisplayName, badRequest.Value);
            //}
            //else if (result is ObjectResult statusCodeResult && statusCodeResult.StatusCode == StatusCodes.Status500InternalServerError)
            //{
            //    Log.ForContext(context.Controller.GetType()).Error(
            //        "Unexpected exception for {Action}: {StatusCode}: {Phrase}",
            //        context.ActionDescriptor.DisplayName, statusCodeResult.StatusCode, statusCodeResult.Value);
            //}

            if (context.Result is ObjectResult objResult)
            {
                switch (objResult.StatusCode)
                {
                    case StatusCodes.Status400BadRequest:
                        Log.ForContext(context.Controller.GetType()).Warning(
                            "Bad Request for {Action}: {Phrase}",
                            context.ActionDescriptor.DisplayName, objResult.Value);
                        break;
                    case StatusCodes.Status500InternalServerError:
                        Log.ForContext(context.Controller.GetType()).Error(
                            "Error for {Action}: {Phrase}",
                            context.ActionDescriptor.DisplayName, objResult.Value);
                        break;
                    default:
                        Log.ForContext(context.Controller.GetType()).Information(
                            "Unexpected response for {Action}: {StatusCode}: {Phrase}",
                            context.ActionDescriptor.DisplayName, objResult.StatusCode, objResult.Value);
                        break;
                }
            }

            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var quoteIdArgument = context.ActionArguments.FirstOrDefault(x => x.Key == _argumentName);

            Log.ForContext(context.Controller.GetType()).Debug(
                "Executing {Action} for {RequestUrl}",
                context.ActionDescriptor.DisplayName, context.HttpContext.Request.GetUri());
        }
    }
}
