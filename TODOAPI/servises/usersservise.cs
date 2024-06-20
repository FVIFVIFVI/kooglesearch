using Microsoft.Extensions.Options;
using MongoDB.Driver;
using usersApi.Models;
using KoogleDatabaseSettingsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsersApi.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<Users> _usersCollection;

        public UsersService(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<Users>(databaseSettings.Value.UsesrCollectionName);
        }

        public async Task<List<Users>> GetAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<Users?> GetAsync(string id) =>
            await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Users newUsers) =>
            await _usersCollection.InsertOneAsync(newUsers);

        public async Task UpdateAsync(string id, Users updatedUsers) =>
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUsers);

        public async Task RemoveAsync(string id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);
    }
}
