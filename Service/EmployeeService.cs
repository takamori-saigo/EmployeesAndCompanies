using AutoMapper;
using Contracts;
using Entities.Exceptions;
using LoggerService;
using Service.Contracts;
using Shared;

namespace Service;

internal sealed class EmployeeService: IEmployeeService
{
    private readonly ILoggerManager _loggerManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
    {
        _loggerManager = loggerManager;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employees = _repositoryManager.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return employeesDto;
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employee = _repositoryManager.Employee.GetEmployee(companyId, employeeId, trackChanges);
        if (employee == null) throw new EmployeeNotFoundException(employeeId);
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }
}