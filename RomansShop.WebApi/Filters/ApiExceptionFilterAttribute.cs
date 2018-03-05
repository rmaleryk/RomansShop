﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RomansShop.WebApi.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public ApiExceptionFilterAttribute()
        {
        }

        public override void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;

            context.Result = new ContentResult
            {
                Content = $"Exception was thrown by invocation {actionName}: \n {exceptionMessage} \n {exceptionStack}"
            };

            context.ExceptionHandled = true;
        }
    }
}