using System.Dynamic;
using Entities;

namespace Contracts;

public interface IDataShaper<T>
{
    IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldString);
    ShapedEntity ShapeData(T entity, string fieldString);
}