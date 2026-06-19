using AutoMapper;
using Contracts;
using LoggerService;
using Service.Contracts;
using Shared;

namespace Service;

public class ServiceManager: IServiceManager
{
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    public ServiceManager(ILoggerManager loggerManager, IRepositoryManager repositoryManager, IMapper mapper, IDataShaper<EmployeeDto> dataShaper)
    {
        _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, loggerManager, mapper));
        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, loggerManager, mapper, dataShaper));
    }
}