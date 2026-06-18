using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
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

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
    {
        var companies = await _repositoryManager.Company.GetAllCompaniesAsync(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreatiionDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);
        _repositoryManager.Company.CreateCompany(companyEntity);
        _repositoryManager.SaveAsync();
        var  companyDto = _mapper.Map<CompanyDto>(companyEntity);
        return companyDto;
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        if (companyIds == null) throw new IdParametersBadRequestException();
        var companyEntities = await _repositoryManager.Company.GetByIdsAsync(companyIds, trackChanges);
        if (companyEntities.Count() != companyIds.Count()) throw new CollectionByIdsRequestException();
        var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        return companyDtos;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreatiionDto> companyCollection)
    {
        if (companyCollection is null) throw new CompanyCollectionBadRequest();
        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntities)
            _repositoryManager.Company.CreateCompany(company);
        _repositoryManager.SaveAsync();
        var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(x => x.Id));
        return (companyCollectionToReturn, ids);
    }

    public async Task DeleteCompanyAsync(Guid companyId)
    {
        var companyEntity = await _repositoryManager.Company.GetCompanyAsync(companyId, false);
        if (companyEntity == null) throw new CompanyNotFoundException(companyId);
        _repositoryManager.Company.DeleteCompany(companyEntity);
        _repositoryManager.SaveAsync();
    }

    public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdateDto, bool trackChanges)
    {
        var companyEntity = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (companyEntity == null) throw new CompanyNotFoundException(companyId);
        _mapper.Map(companyForUpdateDto, companyEntity);
        _repositoryManager.SaveAsync();
    }
}