using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
        await FindAll(trackChanges).OrderBy(x => x.Name).ToListAsync();

    public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) => 
        await FindByCondition(x => x.Id.Equals(companyId), trackChanges).FirstOrDefaultAsync();
    

    public void CreateCompany(Company company) => 
        Create(company);
    

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges) =>
        await FindByCondition(x => companyIds.Contains(x.Id), trackChanges).ToListAsync();

    public void DeleteCompany(Company company)
    {
        Delete(company);
    }
}