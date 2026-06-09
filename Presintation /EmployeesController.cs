using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace Presintation;

[Route("api/companies/{companyId:guid}/employees")]
[ApiController]
public class EmployeesController: ControllerBase
{
    private readonly IServiceManager _service;
    
    public EmployeesController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetEmployees(Guid companyId)
    {
        var companies = _service.EmployeeService.GetEmployees(companyId, false);
        return Ok(companies);
    }

    [HttpGet("{employeeId:guid}")]
    public IActionResult GetEmployees(Guid companyId, Guid employeeId)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, employeeId, false);
        return Ok(employee);
    }
}