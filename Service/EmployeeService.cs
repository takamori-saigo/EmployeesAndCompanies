using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using LoggerService;
using Service.Contracts;
using Shared;
using Shared.RequestParameters;

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

    public async Task<(IEnumerable<EmployeeDto>, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters parameters, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);
        var employees = await _repositoryManager.Employee.GetEmployeesAsync(companyId, parameters, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return (employeesDto, employees.MetaData);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);
        var employee = await _repositoryManager.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto)
    {
        await CheckIfCompanyExists(companyId, false);
        var employee = _mapper.Map<Employee>(employeeForCreationDto);
        _repositoryManager.Employee.CrateEmployeeForCompany(companyId, employee);
        await _repositoryManager.SaveAsync();
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public async Task DeleteEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);
        var employeeForCompany = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        _repositoryManager.Employee.DeleteEmployee(employeeForCompany);
        await _repositoryManager.SaveAsync();
    }

    public async Task UpdateEmployeeAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto,
        bool compTrackChanges, bool employeeTrackeChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);
        var employeeForCompany = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, employeeTrackeChanges);
        _mapper.Map(employeeForUpdateDto, employeeForCompany);
        _repositoryManager.SaveAsync();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id,
        bool compTrackChanges, bool employeeTrackeChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);
        var employeeEntity = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, compTrackChanges);
        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        await _repositoryManager.SaveAsync();
    }
    
    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists
        (Guid companyId, Guid employeeId, bool trackChanges)
    {
        var employeeDb = await _repositoryManager.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);
        if (employeeDb is null) throw new EmployeeNotFoundException(employeeId);
        return employeeDb;
    }
}