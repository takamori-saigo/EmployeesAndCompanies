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
    public async Task<IActionResult> GetAllCompanies()
    {
        var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(false);
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "GetCompany")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _serviceManager.CompanyService.GetCompanyAsync(id, false);
        if (company == null) 
            throw new CompanyNotFoundException(id);
        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreatiionDto companyDto)
    {
        if (companyDto == null) return BadRequest("CompanyForCreationDto is null");
        var company = await _serviceManager.CompanyService.CreateCompanyAsync(companyDto);
        return CreatedAtRoute("GetCompany", new { id = company.Id }, company);
    }
    
    [HttpGet("Collection/({companyIds})", Name = "GetCompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> companyIds)
    {
        var companies = await _serviceManager.CompanyService.GetByIdsAsync(companyIds, false);
        return Ok(companies);
    }

    [HttpPost("Collection")]
    public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreatiionDto> companyDtos)
    {
        var result = await _serviceManager.CompanyService.CreateCompanyCollectionAsync(companyDtos);
        return CreatedAtRoute("GetCompanyCollection", new { companyIds = result.ids }, result.companies);
    }

    [HttpDelete("{companyId:guid}", Name = "DeleteCompany")]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        await _serviceManager.CompanyService.DeleteCompanyAsync(companyId);
        return NoContent();
    }

    [HttpPut("{companyId:guid}")]
    public async Task<IActionResult> UpdateCompany(Guid companyId,[FromBody]CompanyForUpdateDto companyForUpdateDto)
    {
        if (companyForUpdateDto is null)
            return BadRequest("CompanyForUpdateDto object is null");
        await _serviceManager.CompanyService.UpdateCompanyAsync(companyId, companyForUpdateDto, true);
        return NoContent();
    }
}