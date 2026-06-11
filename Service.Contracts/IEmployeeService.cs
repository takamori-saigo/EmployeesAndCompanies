using Entities;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IEmployeeService
{
    IEnumerable<EmployeeDTO> GetEmployees(Guid companyId, bool trackChanges);
    EmployeeDTO GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    EmployeeDTO CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges);
    void DeleteEmployeeForCompany(Guid companyId, Guid guid, bool trackChanges);
    void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdatingDto employeeForUpdatingDto,
        bool compTrackChanges, bool empTackChanges);
}