using Entities;
using Shared.RequestParameters;

namespace Contracts;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    void CrateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployee(Employee employee);
}