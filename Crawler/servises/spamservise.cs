using Microsoft.Extensions.Options;
using MongoDB.Driver;
using spamApi.Models;
using KoogleDatabaseSettingsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpamApi.Services
{
    public class SpamService
    {
        private readonly IMongoCollection<Spam> _spamCollection;

        public SpamService(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _spamCollection = database.GetCollection<Spam>(databaseSettings.Value.SpamCollectionName);
        }

        public async Task<List<Spam>> GetAsync() =>
            await _spamCollection.Find(_ => true).ToListAsync();

        public async Task<Spam?> GetAsync(string id) =>
            await _spamCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Spam newSpam) =>
            await _spamCollection.InsertOneAsync(newSpam);

        public async Task UpdateAsync(string id, Spam updatedSpam) =>
            await _spamCollection.ReplaceOneAsync(x => x.Id == id, updatedSpam);

        public async Task RemoveAsync(string id) =>
            await _spamCollection.DeleteOneAsync(x => x.Id == id);
    }
}
