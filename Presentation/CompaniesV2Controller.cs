using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.RequestParameters;

namespace Presentation;

[ApiVersion("2.0")]
[Route("api/Companies")]
[ApiController]
public class CompaniesV2Controller: ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public CompaniesV2Controller(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _serviceManager.CompanyService
            .GetAllCompaniesAsync(new CompanyParameters(),false);
        return Ok(companies.Item1);
    }
}