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
    public IActionResult GetEmployees(Guid companyId, bool trackChanges)
    {
        var employees = _serviceManager.EmployeeService.GetEmployees(companyId, trackChanges);
        return Ok(employees);
    }
}