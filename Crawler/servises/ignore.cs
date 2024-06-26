using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ignore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using KoogleDatabaseSettingsApi.Models;

namespace ignore.Services
{
    public class IgnoreService
    {
        private readonly IMongoCollection<Ignore> _ignoreCollection;

        public IgnoreService(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _ignoreCollection = database.GetCollection<Ignore>(databaseSettings.Value.IgnoreCollectionName);
        }

        public async Task<List<Ignore>> GetAsync() =>
            await _ignoreCollection.Find(_ => true).ToListAsync();

        public async Task<Ignore?> GetAsync(string id) =>
            await _ignoreCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Ignore newIgnore) =>
            await _ignoreCollection.InsertOneAsync(newIgnore);

        public async Task UpdateAsync(string id, Ignore updatedIgnore) =>
            await _ignoreCollection.ReplaceOneAsync(x => x.Id == id, updatedIgnore);

        public async Task RemoveAsync(string id) =>
            await _ignoreCollection.DeleteOneAsync(x => x.Id == id);

        public async Task AppendToVisitedAsync(string id, string newItem)
        {
            var filter = Builders<Ignore>.Filter.Eq(x => x.Id, id);
            var update = Builders<Ignore>.Update.Push(x => x.visited, newItem);
            await _ignoreCollection.UpdateOneAsync(filter, update);
        }
    }
}
