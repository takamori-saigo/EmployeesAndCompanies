using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;

namespace Presentation;

[Route("api/Companies/{companyId:guid}/[controller]")]
[ApiController]
public class EmployeesController: ControllerBase
{
    private readonly IServiceManager _serviceManager;
    
    public EmployeesController(IServiceManager serviceManager) =>
        _serviceManager = serviceManager;

    [HttpGet]
    public IActionResult GetEmployees(Guid companyId)
    {
        var employees = _serviceManager.EmployeeService.GetEmployees(companyId, false);
        return Ok(employees);
    }

    [HttpGet("{employeeId:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployees(Guid companyId, Guid employeeId)
    {
        var employee = _serviceManager.EmployeeService.GetEmployee(companyId, employeeId, false);
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId,
        [FromBody] EmployeeForCrationDto employeeForCrationDto)
    {
        if (employeeForCrationDto == null) return BadRequest("EmployeeForCrationDto is null");
        var employee = _serviceManager.EmployeeService.CreateEmployeeForCompany(companyId, employeeForCrationDto);
        return CreatedAtRoute("GetEmployeeForCompany", new {companyId, employeeId = employee.id}, employee);
    }

    [HttpDelete("{employeeId:guid}")]
    public IActionResult DeleteEmployee(Guid companyId, Guid employeeId)
    {
        _serviceManager.EmployeeService.DeleteEmployee(companyId, employeeId, false);
        return NoContent();
    }
}