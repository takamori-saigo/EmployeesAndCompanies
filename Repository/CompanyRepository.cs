using Contracts;
using Entities;

namespace Repository;

public class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        return FindAll(trackChanges).OrderBy(x => x.Name).ToList();
    }

    public Company GetCompany(Guid companyId, bool trackChanges)
    {
        return FindByCondition(x => x.Id.Equals(companyId), trackChanges).FirstOrDefault();
    }

    public void CreateCompany(Company company)
    {
        Create(company);
    }
}