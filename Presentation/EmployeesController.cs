using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.RequestParameters;

namespace Presentation;

[Route("api/Companies/{companyId:guid}/[controller]")]
[ApiController]
public class EmployeesController: ControllerBase
{
    private readonly IServiceManager _serviceManager;
    
    public EmployeesController(IServiceManager serviceManager) =>
        _serviceManager = serviceManager;

    [HttpGet]
    public async Task<IActionResult> GetEmployees(Guid companyId, [FromQuery] EmployeeParameters parameters)
    {
        var employees = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, parameters, false);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(employees.metaData));
        return Ok(employees.Item1);
    }
    
    [HttpHead]
    public async Task<IActionResult> GetEmployeesHead(Guid companyId, [FromQuery] EmployeeParameters parameters)
    {
        var employees = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, parameters, false);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(employees.metaData));
        return Ok();
    }

    [HttpGet("{employeeId:guid}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployees(Guid companyId, Guid employeeId)
    {
        var employee = await _serviceManager.EmployeeService.GetEmployeeAsync(companyId, employeeId, false);
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId,
        [FromBody] EmployeeForCreationDto employeeForCreationDto)
    {
        if (employeeForCreationDto == null) return BadRequest("EmployeeForCrationDto is null");
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        var employee = await _serviceManager.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employeeForCreationDto);
        return CreatedAtRoute("GetEmployeeForCompany", new {companyId, employeeId = employee.id}, employee);
    }

    [HttpDelete("{employeeId:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid employeeId)
    {
        _serviceManager.EmployeeService.DeleteEmployeeAsync(companyId, employeeId, false);
        return NoContent();
    }

    [HttpPut("{employeeId:guid}")]
    public async Task<IActionResult> UpdateEmployee(Guid companyId, Guid employeeId, [FromBody]EmployeeForUpdateDto employeeForUpdateDto)
    {
        if (employeeForUpdateDto == null) return BadRequest("employeeForUpdateDto is null");
        await _serviceManager.EmployeeService.UpdateEmployeeAsync(companyId, employeeId, employeeForUpdateDto, false, true);
        return NoContent();
    }
    
    [HttpPatch("{employeeId:guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid employeeId,
        [FromBody]JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc == null) return BadRequest("patchDoc is null");
        var result = await _serviceManager.EmployeeService.GetEmployeeForPatchAsync(companyId, employeeId, false, true);
        patchDoc.ApplyTo(result.employeeToPatch);
        await _serviceManager.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch,result.employeeEntity);
        return NoContent();
    }
}