using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;

namespace Presintation;

[ApiController]
[Route("api/[controller]")]
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
        return Ok(companies);
    }

    [HttpGet("{companyId:guid}", Name = "GetCompanyById")]
    public IActionResult GetCompany(Guid companyId)
    {
        var company = _serviceManager.CompanyService.GetCompany(companyId, false);
        return Ok(company);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
    {
        if (company == null) return BadRequest("Company is null");
        var companyDto = _serviceManager.CompanyService.CreateCompany(company);
        return CreatedAtRoute("GetCompanyById", new { companyId =  companyDto.Id }, companyDto);
    }
}