using AutoMapper;
using Contracts;
using Entities;
using Entities.Exception;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class EmployeeService: IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper =  mapper;
    }

    public IEnumerable<EmployeeDTO> GetEmployees(Guid companyId, bool trackChanges)
    {
        var employees = _repository.Employee.GetEmployees(companyId, trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDTO>>(employees);
        return companiesDto;
    }

    public EmployeeDTO GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employee = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
        if (employee == null) throw new EmployeeNotFoundException(employeeId);
        var employeesDto = _mapper.Map<Employee, EmployeeDTO>(employee);
        return employeesDto;
    }

    public EmployeeDTO CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId,  false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employee = _mapper.Map<Employee>(employeeForCreationDto);
        _repository.Employee.CreateEmployeeForCompany(companyId, employee);
        _repository.Save();
        var employeeDto = _mapper.Map<EmployeeDTO>(employee);
        return employeeDto;
    }
}