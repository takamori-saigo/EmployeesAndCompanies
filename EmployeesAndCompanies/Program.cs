using EmployeesAndCompanies.ServiceExtension;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();
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
app.Run(async context => await context.Response.WriteAsync("Hello Pidr!"));
app.Run(async context => await context.Response.WriteAsync("Huilo"));
app.MapControllers();

app.Run();