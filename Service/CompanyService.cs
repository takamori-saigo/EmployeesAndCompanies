using Contracts;
using LoggerService;
using Service.Contracts;

namespace Service;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;

    public CompanyService(IRepositoryManager repository, ILoggerManager loggerManager)
    {
        _repositoryManager = repository;
        _loggerManager = loggerManager;
    }
}