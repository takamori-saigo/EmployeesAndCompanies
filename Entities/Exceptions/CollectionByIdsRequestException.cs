namespace Entities.Exceptions;

public class CollectionByIdsRequestException: BadRequestException
{
    public CollectionByIdsRequestException() : base("Collection count mismatch comparing to ids")
    {
    }
}