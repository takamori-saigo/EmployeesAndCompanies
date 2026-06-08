using Contracts;
using Service.Contracts;

namespace Service;

public sealed class ServiceManager: IServiceManager
{
    public ICompanyService CompanyService => _company.Value;
    public IEmployeeService EmployeeService => _employee.Value;
    private readonly Lazy<ICompanyService> _company;
    private readonly Lazy<IEmployeeService> _employee;

    public ServiceManager(IRepositoryManager repository, ILoggerManager logger)
    {
        _company = new Lazy<ICompanyService>(() => new CompanyService(repository, logger));
        _employee = new Lazy<IEmployeeService>(() => new EmployeeService(repository, logger));
    }
    
}