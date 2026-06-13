using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Repository;

internal sealed class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext): base(repositoryContext){}
    public async Task<IEnumerable<Company>> GetCompaniesAsync(bool trackChanges)
    {
        return await FindALl(trackChanges).OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        return await FindByCondition(x => x.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();
    }

    public void CreateCompany(Company company)
    {
        Create(company);
    }

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        return await FindByCondition(x => companyIds.Contains(x.Id), trackChanges).ToListAsync();
    }

    public void DeleteCompany(Company company)
    {
        Delete(company);
    }
}