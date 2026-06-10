using Contracts;
using Entities;

namespace Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repository) : base(repository)
    {
    }

    public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)
    {
        return FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges);
    }

    public Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        return FindByCondition(x => x.Id.Equals(employeeId) && x.CompanyId.Equals(companyId), trackChanges)
            .SingleOrDefault();
    }

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee)
    {
        Delete(employee);
    }
}