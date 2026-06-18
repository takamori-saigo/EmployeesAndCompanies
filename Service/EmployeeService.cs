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

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employees = await _repositoryManager.Employee.GetEmployeesAsync(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return employeesDto;
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employee = await _repositoryManager.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);
        if (employee == null) throw new EmployeeNotFoundException(employeeId);
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employee = _mapper.Map<Employee>(employeeForCreationDto);
        _repositoryManager.Employee.CrateEmployeeForCompany(companyId, employee);
        await _repositoryManager.SaveAsync();
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public async Task DeleteEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employeeForCompany = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeForCompany == null) throw new EmployeeNotFoundException(id);
        _repositoryManager.Employee.DeleteEmployee(employeeForCompany);
        _repositoryManager.SaveAsync();
    }

    public async Task UpdateEmployeeAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto,
        bool compTrackChanges, bool employeeTrackeChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employeeForCompany = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, employeeTrackeChanges);
        if (employeeForCompany == null) throw new EmployeeNotFoundException(id);
        _mapper.Map(employeeForUpdateDto, employeeForCompany);
        _repositoryManager.SaveAsync();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id,
        bool compTrackChanges, bool employeeTrackeChanges)
    {
        var compnay = await _repositoryManager.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (compnay == null) throw new CompanyNotFoundException(id);
        var employeeEntity = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, compTrackChanges);
        if (employeeEntity == null) throw new EmployeeNotFoundException(id);
        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        await _repositoryManager.SaveAsync();
    }
}