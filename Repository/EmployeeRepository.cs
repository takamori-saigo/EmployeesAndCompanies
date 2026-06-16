using Contracts;
using Entities;

namespace Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)
    {
        return FindByCondition(x => x.CompanyId.Equals(companyId), trackChanges);
    }
}