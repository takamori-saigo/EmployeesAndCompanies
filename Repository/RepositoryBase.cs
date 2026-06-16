using System.Linq.Expressions;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public abstract class RepositoryBase<T>: IRepositoryBase<T> where T: class
{
    private readonly RepositoryContext _dbContext;
    
    public RepositoryBase(RepositoryContext repositoryContext)
    {
        _dbContext = repositoryContext;
    }
    
    public IQueryable<T> FindAll(bool trackTracking)
    {
        return trackTracking ? _dbContext.Set<T>() : _dbContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackTracking)
    {
        return trackTracking? _dbContext.Set<T>().Where(expression) :
            _dbContext.Set<T>().Where(expression).AsNoTracking();
    }

    public void Create(T model)
    {
        _dbContext.Set<T>().Add(model);
    }

    public void Update(T model)
    {
        _dbContext.Set<T>().Update(model);
    }

    public void Delete(T model)
    {
        _dbContext.Set<T>().Remove(model);
    }
}