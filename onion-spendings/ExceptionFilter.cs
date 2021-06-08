using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Spendings.Core.Exeptions;

namespace onion_spendings
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            if (context.Exception is WrongLoginDataException
                || context.Exception is InvalidOperationException
                || context.Exception is FailedInsertionException
                || context.Exception is OverflowException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (context.Exception is EmptyIntervalException 
                || context.Exception is DeletionFailedException
                || context.Exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else if (context.Exception is AlreadyDeletedException)
            {
                statusCode = HttpStatusCode.Conflict;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.ExceptionHandled = true;
        }
    }
}
