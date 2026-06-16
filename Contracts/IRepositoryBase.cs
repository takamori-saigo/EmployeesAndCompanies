using System.Linq.Expressions;

namespace Contracts;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll(bool trackTracking);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackTracking);
    void Create(T model);
    void Update(T model);
    void Delete(T model);
}