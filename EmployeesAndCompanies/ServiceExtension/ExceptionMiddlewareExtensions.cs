using System.Text;
using System.Text.Json;
using Contracts;
using Entities.Exception;

namespace EmployeesAndCompanies.ServiceExtension;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (NotFoundException ex)
            {
                logger.LogError($"NotFoundException: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";
                var body = JsonSerializer.Serialize(new { StatusCode = 404, Message = "NotFound" });
                await context.Response.WriteAsync(body, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception: {ex}");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var body = JsonSerializer.Serialize(new { StatusCode = 500, Message = "Internal Server Error" });
                await context.Response.WriteAsync(body, Encoding.UTF8);
            }
        });
    }
}
