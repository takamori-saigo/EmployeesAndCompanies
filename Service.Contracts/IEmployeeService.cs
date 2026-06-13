using Entities;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    Task<EmployeeDTO> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task<EmployeeDTO> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges);
    Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid guid, bool trackChanges);
    Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdatingDto employeeForUpdatingDto,
        bool compTrackChanges, bool empTackChanges);
    Task<(EmployeeForUpdatingDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool trackChanges, bool empTrackChanges);

    Task SaveChangesForPatchAsync(EmployeeForUpdatingDto employeeToPatch, Employee employeeEntity);
}