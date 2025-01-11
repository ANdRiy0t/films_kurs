using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace kino_ku.Api.Repository
{
    public interface IGenericRepository<T>
    {

        IEnumerable<T> GetAll();
        Task Add(T user);
        Task Update(T user);
        Task Delete(T user);
        Task<T> Get(ObjectId id);
        Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression,
            SortDefinition<T> sortDefinition = null);
    }
}
