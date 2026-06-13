using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repository) : base(repository)
    {
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        return await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).ToListAsync();
    }

    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        return await FindByCondition(x => x.Id.Equals(employeeId) && x.CompanyId.Equals(companyId), trackChanges)
            .SingleOrDefaultAsync();
    }

    public Task CreateEmployeeForCompanyAsync(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
        return Task.CompletedTask;
    }

    public Task DeleteEmployeeAsync(Employee employee)
    {
        Delete(employee);
        return Task.CompletedTask;
    }
}