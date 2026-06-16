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
        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employees = _repositoryManager.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return employeesDto;
    }
}