using System.Reflection;
using System.Text;
using Entities;

namespace Repository.Extensions;

public static class OrderQueryBuilder
{
    public static string CreateOrderQuery<T>(string orderByQueryString)
    {
        //разделение на ascenting и descending
        var orderParams = orderByQueryString.Trim().Split(',');
        //рефлексия - получение публичных свойств и тех которые доступны после создания
        var propertyInfos = typeof(Employee).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        //перебираем все параметры сортировки
        foreach (var param in orderParams)
        {
            if (string.IsNullOrEmpty(param)) 
                continue;
            
            
            var propertyFromQueryName = param.Split(' ')[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName,
                StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null) continue;
            var direction = param.EndsWith(" desc") ? "descending" :  "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
        }
        return orderByQueryString.ToString().TrimEnd(',',' ');
    }
}