using AutoMapper;
using Contracts;
using Entities;
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

    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employee = _mapper.Map<Employee>(employeeForCreationDto);
        _repositoryManager.Employee.CrateEmployeeForCompany(companyId, employee);
        _repositoryManager.SaveChanges();
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public void DeleteEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company =  _repositoryManager.Company.GetCompany(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employeeForCompany = _repositoryManager.Employee.GetEmployee(companyId, id, trackChanges);
        if (employeeForCompany == null) throw new EmployeeNotFoundException(id);
        _repositoryManager.Employee.DeleteEmployee(employeeForCompany);
        _repositoryManager.SaveChanges();
    }

    public void UpdateEmployee(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto,
        bool compTrackChanges, bool employeeTrackeChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, compTrackChanges);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employeeForCompany = _repositoryManager.Employee.GetEmployee(companyId, id, employeeTrackeChanges);
        if (employeeForCompany == null) throw new EmployeeNotFoundException(id);
        _mapper.Map(employeeForUpdateDto, employeeForCompany);
        _repositoryManager.SaveChanges();
    }

    public (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid id,
        bool compTrackChanges, bool employeeTrackeChanges)
    {
        var compnay = _repositoryManager.Company.GetCompany(companyId, compTrackChanges);
        if (compnay == null) throw new CompanyNotFoundException(id);
        var employeeEntity = _repositoryManager.Employee.GetEmployee(companyId, id, compTrackChanges);
        if (employeeEntity == null) throw new EmployeeNotFoundException(id);
        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        _repositoryManager.SaveChanges();
    }
}