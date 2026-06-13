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

    public void DeleteEmployeeForCompany(Guid companyId, Guid guid, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employeeForCompany = _repository.Employee.GetEmployee(companyId, guid, trackChanges);
        if (employeeForCompany == null) throw new EmployeeNotFoundException(guid);
        _repository.Employee.DeleteEmployee(employeeForCompany);
        _repository.Save();
    }

    public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdatingDto employeeForUpdatingDto,
        bool compTrackChanges, bool empTackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, compTrackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);
        var employeeEntity = _repository.Employee.GetEmployee(companyId, id, empTackChanges);
        if (employeeEntity is null) throw new EmployeeNotFoundException(id);
        _mapper.Map(employeeForUpdatingDto, employeeEntity);
        _repository.Save();
    }

    public (EmployeeForUpdatingDto employeeForUpdatingDto, Employee employee) GetEmployeeForPatch(Guid companyId, Guid id,
        bool trackChanges, bool empTrackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employeeEntity = _repository.Employee.GetEmployee(companyId, id, empTrackChanges);
        if (employeeEntity == null) throw new EmployeeNotFoundException(id);
        var employeeToPatch = _mapper.Map<EmployeeForUpdatingDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public void SaveChangesForPatch(EmployeeForUpdatingDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        _repository.Save();
    }
}