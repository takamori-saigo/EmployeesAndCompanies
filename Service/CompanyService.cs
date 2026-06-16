using AutoMapper;
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
    private readonly IMapper _mapper;
    
    public CompanyService(IRepositoryManager repository, ILoggerManager loggerManager, IMapper mapper)
    {
        _repositoryManager = repository;
        _loggerManager = loggerManager;
        _mapper =  mapper;
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = _repositoryManager.Company.GetAllCompanies(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
        }
        catch(Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            throw;
        }
    }
}