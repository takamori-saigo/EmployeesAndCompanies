using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace Presentation;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController: ControllerBase
{
    private readonly IServiceManager _serviceManager;
    
    public CompaniesController(IServiceManager service)
    {
        _serviceManager =  service;
    }
    
    [HttpGet]
    public IActionResult GetAllCompanies()
    {
        var companies = _serviceManager.CompanyService.GetAllCompanies(false);
        return Ok(companies);
    }

    [HttpGet("{companyId:guid}")]
    public IActionResult GetCompany(Guid companyId)
    {
        var company = _serviceManager.CompanyService.GetCompany(companyId, false);
        return Ok(company);
    }
}