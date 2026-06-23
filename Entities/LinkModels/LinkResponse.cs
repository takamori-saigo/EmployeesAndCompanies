namespace Entities.LinkModels;

public class LinkResponse
{
    public bool HasLinks { get; set; }
    public List<Entity> shapedEntities { get; set; }
    public LinkCollectionWrapper<Entity> LinkCollection { get; set; }

    public LinkResponse()
    {
        LinkCollection = new LinkCollectionWrapper<Entity>();
        shapedEntities = new List<Entity>();
    }
}