using System.Reflection.Metadata;
using Contracts;
using EmployeesAndCompanies.ServiceExtension;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
    }
).AddXmlDataContractSerializerFormatters()
.AddApplicationPart(typeof(Presintation.AssemblyReference).Assembly)
.AddCustomCSVFormatter();
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlService(builder.Configuration.GetConnectionString("DefaultConnection"));
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureLoggerService();
var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerManager>();
if (!app.Environment.IsDevelopment()) app.UseHsts();
app.ConfigureExceptionHandler(logger);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();