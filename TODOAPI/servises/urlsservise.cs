using Microsoft.Extensions.Options;
using MongoDB.Driver;
using urlsApi.Models;
using KoogleDatabaseSettingsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UrlsApi.Services
{
    public class UrlsService
    {
        private readonly IMongoCollection<Urls> _urlsCollection;

        public UrlsService(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _urlsCollection = database.GetCollection<Urls>(databaseSettings.Value.UrlsCollectionName);
        }

        public async Task<List<Urls>> GetAsync() =>
            await _urlsCollection.Find(_ => true).ToListAsync();

        public async Task<Urls?> GetAsync(string id) =>
            await _urlsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Urls newUrls) =>
            await _urlsCollection.InsertOneAsync(newUrls);

        public async Task UpdateAsync(string id, Urls updatedUrls) =>
            await _urlsCollection.ReplaceOneAsync(x => x.Id == id, updatedUrls);

        public async Task RemoveAsync(string id) =>
            await _urlsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
