using AutoMapper;
using Contracts;
using Entities;
using Entities.Exception;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class CompanyService: ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<CompanyDTO> GetAllCompanies(bool trackChanges)
    {
        var companies = _repository.Company.GetCompanies(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDTO>>(companies);
        _logger.LogInfo("GetAllCompanies");
        return companiesDto;
    }

    public CompanyDTO GetCompany(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company == null)
            throw new CompanyNotFoundException(companyId);
        var compantDto =  _mapper.Map<CompanyDTO>(company);
        _logger.LogInfo("GetCompany");
        return compantDto;
    }

    public CompanyDTO CreateCompany(CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);
        _repository.Company.CreateCompany(companyEntity);
        _repository.Save();
        var companyForReturn =  _mapper.Map<CompanyDTO>(companyEntity);
        return companyForReturn;
    }

    public IEnumerable<CompanyDTO> GetByIds(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        if (companyIds == null) throw new IdParametersBadRequestException();
        var companyEntities = _repository.Company.GetByIds(companyIds, trackChanges);
        if (companyEntities.Count() != companyIds.Count())
            throw new CollectionByIdsBadRequestsException();
        var companiesToReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);
        return companiesToReturn;
    }

    public (IEnumerable<CompanyDTO> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection is null) throw new CompanyCollectionBadRequest();
        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntities)
        {
            _repository.Company.CreateCompany(company);
        }
        _repository.Save();
        var companyCollectionReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);
        var ids = string.Join(',', companyCollectionReturn.Select(x => x.Id));
        return (companyCollectionReturn, ids);
    }
}