using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace Presintation;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController: ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public CompaniesController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }
    
    [HttpGet]
    public IActionResult GetCompanies()
    {
        try
        {
            var companies = _serviceManager.CompanyService.GetAllCompanies(false);
            return Ok(companies);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }
}