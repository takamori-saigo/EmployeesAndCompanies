using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Presentation.ModelBinders;
using Service.Contracts;
using Shared;

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

    [HttpGet("{id:guid}", Name = "GetCompany")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _serviceManager.CompanyService.GetCompany(id, false);
        if (company == null) 
            throw new CompanyNotFoundException(id);
        return Ok(company);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreatiionDto companyDto)
    {
        if (companyDto == null) return BadRequest("CompanyForCreationDto is null");
        var company = _serviceManager.CompanyService.CreateCompany(companyDto);
        return CreatedAtRoute("GetCompany", new { id = company.Id }, company);
    }
    
    [HttpGet("Collection/({companyIds})", Name = "GetCompanyCollection")]
    public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> companyIds)
    {
        var companies = _serviceManager.CompanyService.GetByIds(companyIds, false);
        return Ok(companies);
    }

    [HttpPost("Collection")]
    public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreatiionDto> companyDtos)
    {
        var result = _serviceManager.CompanyService.CreateCompanyCollection(companyDtos);
        return CreatedAtRoute("GetCompanyCollection", new { companyIds = result.ids }, result.companies);
    }

    [HttpDelete("{companyId:guid}", Name = "DeleteCompany")]
    public IActionResult DeleteCompany(Guid companyId)
    {
        _serviceManager.CompanyService.DeleteCompany(companyId);
        return NoContent();
    }
}