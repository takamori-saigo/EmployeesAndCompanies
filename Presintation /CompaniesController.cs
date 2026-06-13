using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;

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
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(false);
        return Ok(companies);
    }

    [HttpGet("{companyId:guid}", Name = "GetCompanyById")]
    public async Task<IActionResult> GetCompany(Guid companyId)
    {
        var company = await _serviceManager.CompanyService.GetCompanyAsync(companyId, false);
        return Ok(company);
    }
    
    [HttpDelete("{companyId:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        await _serviceManager.CompanyService.DeleteCompanyAsync(companyId, false);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany(CompanyForCreationDto company)
    {
        if (company == null) return BadRequest("Company is null");
        var companyDto = await _serviceManager.CompanyService.CreateCompanyAsync(company);
        return CreatedAtRoute("GetCompanyById", new { companyId =  companyDto.Id }, companyDto);
    }

    [HttpGet("collection/({ids})", Name = "GetCompaniesByIds")]
    public async Task<IActionResult> GetCompaniesByIds([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = await _serviceManager.CompanyService.GetByIdsAsync(ids, false);
        return Ok(companies);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        var result = await _serviceManager.CompanyService.CreateCompanyCollectionAsync(companyCollection);
        return CreatedAtRoute("GetCompaniesByIds", new { ids = result }, result);
    }

    [HttpPut("{companyId:guid}")]
    public async Task<IActionResult> UpdateCompany(Guid companyId, [FromBody]CompanyForUpdateDto company)
    {
        if (company == null) return BadRequest("CompanyForUpdate is null");
        await _serviceManager.CompanyService.UpdateCompanyAsync(companyId, company, true );
        
        return NoContent();
    }
}
