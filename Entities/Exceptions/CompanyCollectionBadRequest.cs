namespace Entities.Exceptions;

public class CompanyCollectionBadRequest: BadRequestException
{
    public CompanyCollectionBadRequest() : base("Compnay collection sent from a client is null")
    {
    }
}