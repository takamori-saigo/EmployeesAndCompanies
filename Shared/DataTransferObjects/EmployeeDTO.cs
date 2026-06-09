namespace Shared.DataTransferObjects;

[Serializable]
public record class EmployeeDTO(Guid id, string Name, int Age, string Position);

