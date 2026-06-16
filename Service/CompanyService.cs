using Contracts;
using Entities;
using LoggerService;
using Service.Contracts;
using Shared;

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

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = _repositoryManager.Company.GetAllCompanies(trackChanges);
            return companies.Select(x => new CompanyDto(x.Id,x.Name,$"{x.Country} {x.Address}"));
        }
        catch(Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            throw;
        }
    }
}