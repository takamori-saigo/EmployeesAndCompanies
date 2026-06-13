using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presintation;

[Route("api/companies/{companyId:guid}/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;

    public EmployeesController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees(Guid companyId)
    {
        var employees = await _service.EmployeeService.GetEmployeesAsync(companyId, false);
        return Ok(employees);
    }

    [HttpGet("{employeeId:guid}", Name = "GetEmployeesForCompany")]
    public async Task<IActionResult> GetEmployees(Guid companyId, Guid employeeId)
    {
        var employee = await _service.EmployeeService.GetEmployeeAsync(companyId, employeeId, false);
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(Guid companyId, EmployeeForCreationDto employeeForCreationDto)
    {
        if (employeeForCreationDto is null)
            return BadRequest("employeeForCreationDto object is null");
        var employeeToReturn =
            await _service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employeeForCreationDto, false);
        return CreatedAtRoute("GetEmployeesForCompany", new { companyId = companyId, employeeId = employeeToReturn.id },
            employeeToReturn);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        await _service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] EmployeeForUpdatingDto employeeForUpdatingDto)
    {
        if (employeeForUpdatingDto is null) return BadRequest("EmployeeForCompany is null");
        await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employeeForUpdatingDto, false, true);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdatingDto> patchDoc)
    {
        if (patchDoc == null) return BadRequest("patchDoc object sent from client is null");
        var result = await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, id, false, true);
        patchDoc.ApplyTo(result.employeeToPatch);
        await _service.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);
        return NoContent();
    }
}
