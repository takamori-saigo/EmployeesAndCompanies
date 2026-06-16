namespace Entities.Exceptions;

public abstract class NotFoundException: Exception
{
    public  NotFoundException(string msg) : base(msg) { }
}