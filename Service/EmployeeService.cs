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

    public async Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        var employees = await _repository.Employee.GetEmployeesAsync(companyId, trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDTO>>(employees);
        return companiesDto;
    }

    public async Task<EmployeeDTO> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employee = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);
        if (employee == null) throw new EmployeeNotFoundException(employeeId);
        var employeesDto = _mapper.Map<Employee, EmployeeDTO>(employee);
        return employeesDto;
    }

    public async Task<EmployeeDTO> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId,  false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employee = _mapper.Map<Employee>(employeeForCreationDto);
        await _repository.Employee.CreateEmployeeForCompanyAsync(companyId, employee);
        await _repository.SaveAsync();
        var employeeDto = _mapper.Map<EmployeeDTO>(employee);
        return employeeDto;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid guid, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employeeForCompany = await _repository.Employee.GetEmployeeAsync(companyId, guid, trackChanges);
        if (employeeForCompany == null) throw new EmployeeNotFoundException(guid);
        await _repository.Employee.DeleteEmployeeAsync(employeeForCompany);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdatingDto employeeForUpdatingDto,
        bool compTrackChanges, bool empTackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);
        var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, empTackChanges);
        if (employeeEntity is null) throw new EmployeeNotFoundException(id);
        _mapper.Map(employeeForUpdatingDto, employeeEntity);
        await _repository.SaveAsync();
    }

    public async Task<(EmployeeForUpdatingDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id,
        bool trackChanges, bool empTrackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
        if (employeeEntity == null) throw new EmployeeNotFoundException(id);
        var employeeToPatch = _mapper.Map<EmployeeForUpdatingDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdatingDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        await _repository.SaveAsync();
    }
}