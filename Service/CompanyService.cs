using AutoMapper;
using Contracts;
using Entities;
using Entities.Exception;
using Service.Contracts;
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
}