using Contracts;
using Entities;

namespace Repository;

public class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges).OrderBy(x => x.Name).ToList();
    

    public Company GetCompany(Guid companyId, bool trackChanges) => 
        FindByCondition(x => x.Id.Equals(companyId), trackChanges).FirstOrDefault();
    

    public void CreateCompany(Company company) => 
        Create(company);
    

    public IEnumerable<Company> GetByIds(IEnumerable<Guid> companyIds, bool trackChanges) =>
        FindByCondition(x => companyIds.Contains(x.Id), trackChanges).ToList();

    public void DeleteCompany(Company company)
    {
        Delete(company);
    }
}