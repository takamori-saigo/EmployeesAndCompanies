namespace Entities.Exception;

public class CompanyNotFoundException: NotFoundException
{
    public CompanyNotFoundException(Guid company) : base($"company with id {company} was not found")
    {
    }
}