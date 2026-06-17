namespace Shared;

public record EmployeeDto
{
    public Guid id{ get; init; }
    public string Name{ get; init; }
    public int Age{ get; init; }
    public string Position { get; init; }
};