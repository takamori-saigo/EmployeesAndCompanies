using Entities;
using Shared;
using Shared.RequestParameters;

namespace Service.Contracts;

public interface ICompanyService
{
    Task<(IEnumerable<CompanyDto>, MetaData metaData)> GetAllCompaniesAsync(CompanyParameters parameters, bool trackChanges);
    Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);
    Task<CompanyDto> CreateCompanyAsync(CompanyForCreatiionDto company);
    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> companyIds,bool trackChanges);
    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync
        (IEnumerable<CompanyForCreatiionDto> companyCollection);
    Task DeleteCompanyAsync(Guid companyId);
    Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdateDto, bool trackChanges);
}