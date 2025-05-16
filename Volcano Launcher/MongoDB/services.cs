using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;
using System.Text.Json;

namespace Volcano_Launcher.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<BsonDocument> _usersCollection;

        public MongoDBService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _usersCollection = database.GetCollection<BsonDocument>("users");
        }

        public async Task<string> GetUsernameByEmail(string email)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("email", email);
            var user = await _usersCollection.Find(filter).FirstOrDefaultAsync();

            if (user != null)
            {
                return user["username"].ToString();
            }

            return null;
        }
    }
}
