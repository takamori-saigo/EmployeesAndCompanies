using Entities;
using Shared.RequestParameters;

namespace Contracts;

public interface ICompanyRepository
{
    Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters parameters, bool trackChanges);
    Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> companyIds,bool trackChanges);
    void DeleteCompany(Company company);
}