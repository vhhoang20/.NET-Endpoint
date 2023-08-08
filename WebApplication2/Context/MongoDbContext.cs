using MongoDB.Driver;

namespace WebApplication2.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDBConnection");
            var mongoClient = new MongoClient(connectionString);
            _database = mongoClient.GetDatabase("YourDatabaseName");
        }
    }
}
