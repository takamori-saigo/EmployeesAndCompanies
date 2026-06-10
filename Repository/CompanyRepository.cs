using Contracts;
using Entities;
using Shared;

namespace Repository;

internal sealed class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext): base(repositoryContext){}
    public IEnumerable<Company> GetCompanies(bool trackChanges)
    {
        return FindALl(trackChanges).OrderBy(x => x.Name).ToList();
    }

    public Company GetCompany(Guid companyId, bool trackChanges)
    {
        return FindByCondition(x => x.Id.Equals(companyId), trackChanges).SingleOrDefault();
    }

    public void CreateCompany(Company company)
    {
        Create(company);
    }
}