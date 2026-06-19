using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestParameters;

namespace Repository;

public class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters parameters, bool trackChanges)
    {
        var companies = await FindAll(trackChanges).OrderBy(x => x.Name)
            .Search(parameters.Name)
            .ToListAsync();
        return new PagedList<Company>(companies, companies.Count, parameters.PageNumber, parameters.PageSize);
    }

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