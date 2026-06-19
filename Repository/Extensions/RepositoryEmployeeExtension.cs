using System.Reflection;
using System.Text;
using Entities;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions;

public static class RepositoryEmployeeExtension
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees,
        uint minAge, uint maxAge) => employees.Where(x => x.Age >= minAge && x.Age <= maxAge);

    public static IQueryable<Employee> Search(this IQueryable<Employee> employee, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return employee;

        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return employee.Where(x => x.Name.ToLower().Contains(lowerCaseTerm));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
    {
        //проверка является ли orderByQueryString пустотой
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return employees.OrderBy(x => x.Name);

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

        var oderQuery = orderQueryBuilder.ToString().TrimEnd(',',' ');
        if (string.IsNullOrWhiteSpace(oderQuery))
            return employees.OrderBy(e => e.Name);
        return employees.OrderBy(oderQuery);
    }
}