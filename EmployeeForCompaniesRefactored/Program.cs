using EmployeeForCompaniesRefactored.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.AddConfigureSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddConfigureRepositoryManager();
builder.Services.ConfigureLoggerService();
builder.Services.AddConfigureServiceManager();
builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
var app = builder.Build();
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
else app.UseHsts();
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