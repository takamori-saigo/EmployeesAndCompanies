namespace Entities.Exception;

public class CollectionByIdsBadRequestsException: BadRequestException
{
    public CollectionByIdsBadRequestsException() : base("Collection count mismatch comparing to ids")
    {
    }
}