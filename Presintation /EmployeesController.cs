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

    public IActionResult GetEmployees(Guid companyId)
    {
        var companies = _service.EmployeeService.GetEmployees(companyId, false);
        return Ok(companies);
    }

    [HttpGet("{employeeId:guid}", Name = "GetEmployeesForCompany")]
    public IActionResult GetEmployees(Guid companyId, Guid employeeId)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, employeeId, false);
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployee(Guid companyId, EmployeeForCreationDto employeeForCreationDto)
    {
        if (employeeForCreationDto is null)
            return BadRequest("employeeForCreationDto object is null");
        var employeeToReturn =
            _service.EmployeeService.CreateEmployeeForCompany(companyId, employeeForCreationDto, false);
        return CreatedAtRoute("GetEmployeesForCompany", new { companyId = companyId, employeeId = employeeToReturn.id },
            employeeToReturn);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        _service.EmployeeService.DeleteEmployeeForCompany(companyId, id, false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] EmployeeForUpdatingDto employeeForUpdatingDto)
    {
        if (employeeForUpdatingDto is null) return BadRequest("EmployeeForCompany is null");
        _service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employeeForUpdatingDto, false, true);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdatingDto> patchDoc)
    {
        if (patchDoc == null) return BadRequest("patchDoc object sent from client is null");
        var result = _service.EmployeeService.GetEmployeeForPatch(companyId, id, false, true);
        patchDoc.ApplyTo(result.employeeForUpdatingDto);
        _service.EmployeeService.SaveChangesForPatch(result.employeeForUpdatingDto, result.employee);
        return NoContent();
    }
}