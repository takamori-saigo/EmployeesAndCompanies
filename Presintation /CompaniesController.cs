using Contracts;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace Presintation;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController: ControllerBase
{
    private readonly IServiceManager _serviceManager;
    private readonly ILoggerManager _logger;
    public CompaniesController(IServiceManager serviceManager, ILoggerManager logger)
    {
        _serviceManager = serviceManager;
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult GetCompanies()
    {
        var companies = _serviceManager.CompanyService.GetAllCompanies(false);
        _logger.LogInfo("Companies retrieved");
        return Ok(companies);
    }

    [HttpGet("{companyId:guid}")]
    public IActionResult GetCompany(Guid companyId)
    {
        var company = _serviceManager.CompanyService.GetCompany(companyId, false);
        return Ok(company);
    }
}