using System.Net;
using Entities.ErrorDetails;
using Entities.Exceptions;
using LoggerService;
using Microsoft.AspNetCore.Diagnostics;

namespace EmployeeForCompaniesRefactored.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
    {
        app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                    {
                        context.Response.ContentType = "application/json";
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            context.Response.StatusCode = contextFeature.Error switch
                            {
                                NotFoundException => StatusCodes.Status404NotFound,
                                _ => StatusCodes.Status500InternalServerError
                            };
                            logger.LogError($"something went wrong: {contextFeature.Error}");
                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = contextFeature.Error.Message
                            }.ToString());
                        }
                    }
                    );
            }
        );
    }
}