using Furni.Api.Mongo.Settings;
using kino_ku.Api.Entities;

namespace kino_ku.Api.Configuration;

public static class DependencyStartup
{
	public static void AddDependency(this WebApplicationBuilder builder)
	{
		builder.Configuration.SetDatabaseSettings();

		builder.Services.AddMongo()
			.AddMongoRepository<User>("Users")
			.AddMongoRepository<Movie>("Movies")
			.AddMongoRepository<Ticket>("Tickets");
	}

	private static void SetDatabaseSettings(this IConfiguration configuration)
	{
		DatabaseSettings.ConnectionString = configuration.GetValue<string>("ConnectionString:MongoDB");
		DatabaseSettings.DatabaseName = configuration.GetValue<string>("ConnectionString:DataBase");
		DatabaseSettings.CollectionNameUser = configuration.GetValue<string>("MongoCollections:Users");
		DatabaseSettings.CollectionNameMovies = configuration.GetValue<string>("MongoCollections:Movies");
		DatabaseSettings.CollectionNameTickets = configuration.GetValue<string>("MongoCollections:Tickets");
	}
}

