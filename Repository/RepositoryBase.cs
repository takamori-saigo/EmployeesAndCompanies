using System.Linq.Expressions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly RepositoryContext _repository;

    public RepositoryBase(RepositoryContext repository)
    {
        _repository =  repository;
    }
    
    public IQueryable<T> FindALl(bool trackChanges)
    {
        return trackChanges ? _repository.Set<T>() : _repository.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
    {
        return trackChanges ? _repository.Set<T>().Where(expression) : _repository.Set<T>().Where(expression).AsNoTracking();
    }

    public void Create(T model)
    {
        _repository.Set<T>().Add(model);
    }

    public void Update(T model)
    {
        _repository.Set<T>().Update(model);
    }

    public void Delete(T model)
    {
        _repository.Set<T>().Remove(model);
    }
}