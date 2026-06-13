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

    public async Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync(bool trackChanges)
    {
        var companies = await _repository.Company.GetCompaniesAsync(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDTO>>(companies);
        _logger.LogInfo("GetAllCompanies");
        return companiesDto;
    }

    public async Task<CompanyDTO> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company == null)
            throw new CompanyNotFoundException(companyId);
        var compantDto =  _mapper.Map<CompanyDTO>(company);
        _logger.LogInfo("GetCompany");
        return compantDto;
    }

    public async Task<CompanyDTO> CreateCompanyAsync(CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);
        _repository.Company.CreateCompany(companyEntity);
        await _repository.SaveAsync();
        var companyForReturn =  _mapper.Map<CompanyDTO>(companyEntity);
        return companyForReturn;
    }

    public async Task<IEnumerable<CompanyDTO>> GetByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        if (companyIds == null) throw new IdParametersBadRequestException();
        var companyEntities = await _repository.Company.GetByIdsAsync(companyIds, trackChanges);
        if (companyEntities.Count() != companyIds.Count())
            throw new CollectionByIdsBadRequestsException();
        var companiesToReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);
        return companiesToReturn;
    }

    public async Task<(IEnumerable<CompanyDTO> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection is null) throw new CompanyCollectionBadRequest();
        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntities)
        {
            _repository.Company.CreateCompany(company);
        }
        await _repository.SaveAsync();
        var companyCollectionReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);
        var ids = string.Join(',', companyCollectionReturn.Select(x => x.Id));
        return (companyCollectionReturn, ids);
    }

    public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
    {
        var companyEntity = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if  (companyEntity == null)
            throw new CompanyNotFoundException(companyId);
        _repository.Company.DeleteCompany(companyEntity);
        await _repository.SaveAsync();
    }

    public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdatingDto, bool trackChanges)
    {
        var companyEntity = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (companyEntity == null) throw new CompanyNotFoundException(companyId);
        _mapper.Map(companyForUpdatingDto, companyEntity);
        companyEntity.Employees ??= new List<Employee>();
        foreach (var empDto in companyForUpdatingDto.Employee)                                      
        {                                                                                           
            var newEmployee = _mapper.Map<Employee>(empDto);                                        
            companyEntity.Employees.Add(newEmployee);                                               
        }    
        
        await _repository.SaveAsync();
    }
}