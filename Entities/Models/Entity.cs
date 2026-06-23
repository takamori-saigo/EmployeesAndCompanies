using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Entities.LinkModels;

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
            if (value.GetType() == typeof(List<Link>))
            {
                WriteLinksToXml(key, value, writer);
            }
            else
            {
                writer.WriteString(value.ToString());
            }
            writer.WriteEndElement();
        }
    }

    private void WriteLinksToXml(string key, object value, XmlWriter writer)
    {
        writer.WriteStartElement(key);
        if (value.GetType() == typeof(List<Link>))
        {
            foreach (var val in (value as List<Link>))
            {
                writer.WriteStartElement(nameof(Link));
                WriteLinksToXml(nameof(val.Href), val.Href, writer);
                WriteLinksToXml(nameof(val.Method), val.Method, writer);
                WriteLinksToXml(nameof(val.Rel), val.Rel, writer);
                writer.WriteEndElement();
            }
        }
        else
        {
            writer.WriteString(value.ToString());
        }
        writer.WriteEndElement();
    }
}