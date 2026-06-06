using System.Linq.Expressions;

namespace Contracts;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindALl(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
    void Create(T model);
    void Update(T model);
    void Delete(T model);
}