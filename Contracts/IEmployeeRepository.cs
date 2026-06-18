using Entities;

namespace Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    void CrateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployee(Employee employee);
}