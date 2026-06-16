using EmployeeForCompaniesRefactored.Extensions;
using LoggerService;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.AddConfigureSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddConfigureRepositoryManager();
builder.Services.ConfigureLoggerService();
builder.Services.AddConfigureServiceManager();
builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
        }
    ).AddXmlSerializerFormatters()
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);
if (app.Environment.IsProduction()) 
    app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.All
    }
);
app.UseAuthorization();

app.MapControllers();
app.Run();