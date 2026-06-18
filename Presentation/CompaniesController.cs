using System.Text.Json;
using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Presentation.ModelBinders;
using Service.Contracts;
using Shared;
using Shared.RequestParameters;

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
    public async Task<IActionResult> GetAllCompanies([FromQuery]CompanyParameters companyParameters)
    {
        var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(companyParameters, false);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(companies.metaData));
        return Ok(companies.Item1);
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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreatiionDto companyDto)
    {
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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid companyId,[FromBody]CompanyForUpdateDto companyForUpdateDto)
    {
        await _serviceManager.CompanyService.UpdateCompanyAsync(companyId, companyForUpdateDto, true);
        return NoContent();
    }
}