using Shared;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync(bool trackChanges);
    Task<CompanyDTO> GetCompanyAsync(Guid companyId, bool trackChanges);
    Task<CompanyDTO> CreateCompanyAsync(CompanyForCreationDto company);
    Task<IEnumerable<CompanyDTO>> GetByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges);
    Task<(IEnumerable<CompanyDTO> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companies);
    Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
    Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdatingDto, bool trackChanges);
}