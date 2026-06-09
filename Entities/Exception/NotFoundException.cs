namespace Entities.Exception;

public abstract class NotFoundException: System.Exception
{
    protected NotFoundException(string message) : base(message) { }
}