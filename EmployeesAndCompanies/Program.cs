using Contracts;
using EmployeesAndCompanies.ServiceExtension;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
    }
).AddXmlDataContractSerializerFormatters()
.AddNewtonsoftJson()
.AddApplicationPart(typeof(Presintation.AssemblyReference).Assembly)
.AddCustomCSVFormatter();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(e => e.Key, e => e.Value?.Errors.Select(x => x.ErrorMessage));
        return new UnprocessableEntityObjectResult(new { Errors = errors });
    };
});

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