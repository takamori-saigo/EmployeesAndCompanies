using Entities;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IEmployeeService
{
    IEnumerable<EmployeeDTO> GetEmployees(Guid companyId, bool trackChanges);
    EmployeeDTO GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
}