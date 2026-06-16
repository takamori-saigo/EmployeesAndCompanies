using Contracts;
using Entities;
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

    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = _repositoryManager.Company.GetAllCompanies(trackChanges);
            return companies;
        }
        catch(Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            throw;
        }
    }
}