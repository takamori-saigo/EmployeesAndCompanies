using Contracts;
using Entities;

namespace Repository;

internal sealed class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext): base(repositoryContext){}
    public IEnumerable<Company> GetCompanies(bool trackChanges)
    {
        return FindALl(trackChanges).OrderBy(x => x.Name).ToList();
    }
}