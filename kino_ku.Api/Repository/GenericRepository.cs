using System.Linq.Expressions;
using kino_ku.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace kino_ku.Api.Repository;

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> mongoCollection;
        
        public GenericRepository(IMongoDatabase database, string collectionName)
        {
            mongoCollection = database.GetCollection<T>(collectionName);
        }
        
        public async Task Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await mongoCollection.InsertOneAsync(entity);
        }

        public async Task Delete(T user)
        {
            await mongoCollection.DeleteOneAsync(u => u.Id == user.Id);
        }

        public async Task<T> Get(ObjectId id)
        {
            return await mongoCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public IEnumerable<T> GetAll()
        {
            return mongoCollection.Find(new BsonDocument()).ToList();
        }

        public async Task Update(T user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await mongoCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }
        
        public virtual async Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression,
            SortDefinition<T> sortDefinition = null)
        {
            var findOptions = new FindOptions<T>
            {
                Sort = sortDefinition
            };

            var documents = await mongoCollection.FindAsync(filterExpression, findOptions);
            var document = await documents.FirstOrDefaultAsync();
            return document;
        }
    }
