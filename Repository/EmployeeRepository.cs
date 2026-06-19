using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Shared.RequestParameters;

namespace Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters parameters, bool trackChanges)
    {
        var employees =  await FindByCondition(x => x.CompanyId.Equals(companyId) &&
                x.Age >= parameters.minAge && x.Age <= parameters.maxAge, trackChanges)
            .OrderBy(x => x.Name)
            .ToListAsync();
        return PagedList<Employee>.ToPageList(employees, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        return await FindByCondition(x => x.Id.Equals(employeeId) && x.CompanyId.Equals(companyId), trackChanges)
            .FirstOrDefaultAsync();
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