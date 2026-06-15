using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace EmployeeForCompaniesRefactored.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
        });

    public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options =>
        {
            
        });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();
    
    public static void AddConfigureSqlServer(this IServiceCollection services, string connectionString) =>
        services.AddDbContext<RepositoryContext>(opt =>
            opt.UseNpgsql(connectionString));
}