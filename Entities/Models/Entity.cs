using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Entities;

public class Entity: Dictionary<string, object>, IXmlSerializable
{
    private string _root = "Entity";

    public Entity(){}
    public Entity(string root)
    {
        _root = root;
    }
    
    public XmlSchema? GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.ReadStartElement(_root); //Пропускаем стартовый тег 
        while (!reader.Name.Equals(_root))
        {
            var name = reader.Name;
            reader.MoveToElement();
            Add(name, reader.ReadInnerXml());
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (var key in Keys)
        {
            var value = this[key];
            writer.WriteStartElement(key);
            writer.WriteString(value.ToString());
            writer.WriteEndElement();
        }
    }
}