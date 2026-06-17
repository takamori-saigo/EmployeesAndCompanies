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

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        var companies = _repositoryManager.Company.GetAllCompanies(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public CompanyDto GetCompany(Guid companyId, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges);
        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public CompanyDto CreateCompany(CompanyForCreatiionDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);
        _repositoryManager.Company.CreateCompany(companyEntity);
        _repositoryManager.SaveChanges();
        var  companyDto = _mapper.Map<CompanyDto>(companyEntity);
        return companyDto;
    }

    public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        if (companyIds == null) throw new IdParametersBadRequestException();
        var companyEntities = _repositoryManager.Company.GetByIds(companyIds, trackChanges);
        if (companyEntities.Count() != companyIds.Count()) throw new CollectionByIdsRequestException();
        var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        return companyDtos;
    }

    public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreatiionDto> companyCollection)
    {
        if (companyCollection is null) throw new CompanyCollectionBadRequest();
        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntities)
            _repositoryManager.Company.CreateCompany(company);
        _repositoryManager.SaveChanges();
        var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(x => x.Id));
        return (companyCollectionToReturn, ids);
    }
}