using Contracts;
using LoggerService;
using Service.Contracts;

namespace Service;

internal sealed class EmployeeService: IEmployeeService
{
    private readonly ILoggerManager _loggerManager;
    private readonly IRepositoryManager _repositoryManager;

    public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager)
    {
        _loggerManager = loggerManager;
        _repositoryManager = repositoryManager;
    }
}