using Entities;
using Shared;

namespace Contracts;

public interface ICompanyRepository
{
    IEnumerable<Company> GetCompanies(bool trackChanges);
    Company GetCompany(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
}