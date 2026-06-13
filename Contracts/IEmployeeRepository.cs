using Entities;

namespace Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    
    Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task CreateEmployeeForCompanyAsync(Guid companyId, Employee employee);
    Task DeleteEmployeeAsync(Employee employee);
}