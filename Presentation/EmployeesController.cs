using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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

    [HttpGet("{employeeId:guid}")]
    public IActionResult GetEmployees(Guid companyId, Guid employeeId)
    {
        var employee = _serviceManager.EmployeeService.GetEmployee(companyId, employeeId, false);
        return Ok(employee);
    } 
}