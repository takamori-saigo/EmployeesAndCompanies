namespace Entities.Exception;

public class CompanyCollectionBadRequest: BadRequestException
{
    public CompanyCollectionBadRequest() : base("Company collection sent from a client is null")
    {
    }
}