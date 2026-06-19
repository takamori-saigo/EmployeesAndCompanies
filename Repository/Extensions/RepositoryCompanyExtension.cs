using Entities;

namespace Repository.Extensions;

public static class RepositoryCompanyExtension
{
    public static IQueryable<Company> Search(this IQueryable<Company> query, string searchString)
    {
        return query.Where(company => company.Name.Contains(searchString));
    }
}