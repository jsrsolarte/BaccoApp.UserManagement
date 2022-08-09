using System.Net;
using BaccoApp.UserManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaccoApp.UserManagement.Api.Filters;

[AttributeUsage(AttributeTargets.All)]
public class AppExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<AppExceptionFilterAttribute> _logger;

    public AppExceptionFilterAttribute(ILogger<AppExceptionFilterAttribute> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext? context)
    {
        if (context == null) return;
        object? innerMessage = null;
        if (context.Exception is AppException ex)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            innerMessage = ex.InnerMessage;
        }
        else
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        var genericError = new { ErrorMessage = context.Exception.Message, InnerMessage = innerMessage };
        _logger.LogError("{Exception}: {Message} - {StackTrace}", context.Exception, context.Exception.Message,
            context.Exception.StackTrace);

        context.Result = new ObjectResult(genericError);
    }
}