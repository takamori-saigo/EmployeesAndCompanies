namespace Entities.Exception;

public class EmployeeNotFoundException: NotFoundException
{
    public EmployeeNotFoundException(Guid id) : base($"employee {id} not found")
    {
    }
}