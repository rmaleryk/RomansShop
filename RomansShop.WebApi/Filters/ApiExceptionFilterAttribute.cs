using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ILoggerFactory = RomansShop.Core.Extensibility.ILoggerFactory;
using ILogger = RomansShop.Core.Extensibility.ILogger;

namespace RomansShop.WebApi.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public ApiExceptionFilterAttribute(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public override void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;

            string message = $"Exception was thrown by invocation {actionName}: \n {exceptionMessage} \n {exceptionStack}";

            context.Result = new ContentResult
            {
                Content = message
            };

            context.ExceptionHandled = true;

            _logger.Error(message);
        }
    }
}