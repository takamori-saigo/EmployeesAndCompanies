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
        var oderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);
        if (string.IsNullOrWhiteSpace(oderQuery))
            return employees.OrderBy(e => e.Name);
        return employees.OrderBy(oderQuery);
    }
}