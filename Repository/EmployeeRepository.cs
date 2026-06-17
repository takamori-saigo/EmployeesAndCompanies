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

    public Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        return FindByCondition(x => x.Id.Equals(employeeId) && x.CompanyId.Equals(companyId), trackChanges).FirstOrDefault();
    }

    public void CrateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee)
    {
        Delete(employee);
    }
}