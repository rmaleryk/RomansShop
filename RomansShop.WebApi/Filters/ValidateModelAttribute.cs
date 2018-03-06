using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ILoggerFactory = RomansShop.Core.Extensibility.ILoggerFactory;
using LoggerFactory = RomansShop.Core.Logger.LoggerFactory;
using ILogger = RomansShop.Core.Extensibility.ILogger;
using Newtonsoft.Json;

namespace RomansShop.WebApi.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public ValidateModelAttribute(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger(GetType());
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                _logger.Info("Model is invalid: " + JsonConvert.SerializeObject(context.Result));
            }
        }
    }
}