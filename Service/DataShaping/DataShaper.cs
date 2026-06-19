using System.Dynamic;
using System.Reflection;
using Contracts;

namespace Service.DataShaping;

public class DataShaper<T>: IDataShaper<T> where T : class
{
    public PropertyInfo[] Properties { get; set; }

    public DataShaper()
    {
        Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);    
    }
    
    public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);
        return FetchData(entities, requiredProperties);
    }

    public ExpandoObject ShapeData(T entity, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);
        return FetchDataForEntity(entity, requiredProperties);
    }

    private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
    {
        var requiredProperties = new List<PropertyInfo>();
        if (!string.IsNullOrWhiteSpace(fieldsString))
        {
            var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var field in fields)
            {
                var property = Properties.FirstOrDefault(pi => pi.Name.Equals(field.Trim(),
                    StringComparison.InvariantCultureIgnoreCase));
                if (property == null) continue;
                requiredProperties.Add(property);
            }
        }
        else
        {
            requiredProperties = Properties.ToList();
        }
        return requiredProperties;
    }

    private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapeData = new List<ExpandoObject>();
        foreach (var e in entities)
        {
            var shapedObject = FetchDataForEntity(e, requiredProperties);
            shapeData.Add(shapedObject);
        }
        return shapeData;
    }

    private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedObject = new ExpandoObject();
        foreach (var p in requiredProperties)
        {
            var objectPropertyValue = p.GetValue(entity);
            shapedObject.TryAdd(p.Name, objectPropertyValue);
        }
        return shapedObject;
    }
}