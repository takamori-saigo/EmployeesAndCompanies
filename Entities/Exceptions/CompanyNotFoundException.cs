namespace Entities.Exceptions;

public class CompanyNotFoundException: NotFoundException
{
    public CompanyNotFoundException(Guid companyId) :
        base($"Company with id {companyId} does not exist.") { }
}