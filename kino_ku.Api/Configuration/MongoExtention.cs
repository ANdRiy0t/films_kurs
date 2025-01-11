using Furni.Api.Mongo.Settings;
using kino_ku.Api.Entities;
using kino_ku.Api.Repository;
using MongoDB.Driver;

namespace kino_ku.Api.Configuration
{
	public static class MongoExtention
	{

		public static IServiceCollection AddMongo(this IServiceCollection services)
		{

			services.AddSingleton(serviceProvider =>
			{ 
				var configuration = serviceProvider.GetService<IConfiguration>();
				var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
				var mongoClient = new MongoClient(DatabaseSettings.ConnectionString);
				return mongoClient.GetDatabase(serviceSettings.ServiceName);
			});
			return services;
		}

		public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : BaseEntity
		{
			services.AddSingleton<IGenericRepository<T>>(serviceProvider =>
			{
				var database = serviceProvider.GetService<IMongoDatabase>();
				return new GenericRepository<T>(database, collectionName);
			});

			return services;
		}
	}
}
