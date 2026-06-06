
using Contracts;
using LoggerService;
using NLog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace EmployeesAndCompanies.ServiceExtension;

public static class ServicesExtension
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(x => x.AddPolicy("CorsPolicy",
            policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
    }

    public static void ConfigureIISIntegration(this IServiceCollection services)
    {
        services.Configure<IISOptions>(options =>
        {
            
        });
    }
    
    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }
}