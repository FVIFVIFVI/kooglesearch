using Microsoft.Extensions.Options;
using MongoDB.Driver;
using baseUrlApi.Models;
using KoogleDatabaseSettingsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseUrlApi.Services
{
    public class BaseUrlService
    {
        private readonly IMongoCollection<BaseUrl> _baseUrlCollection;

        public BaseUrlService(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
                if (databaseSettings == null || string.IsNullOrWhiteSpace(databaseSettings.Value.ConnectionString))
                {
                    throw new ArgumentNullException(nameof(databaseSettings), "Database settings or connection string cannot be null or empty.");
                }
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _baseUrlCollection = database.GetCollection<BaseUrl>(databaseSettings.Value.BaseUrlCollectionName);
        }

        public async Task<List<BaseUrl>> GetAsync() =>
            await _baseUrlCollection.Find(_ => true).ToListAsync();

        public async Task<BaseUrl?> GetAsync(string id) =>
            await _baseUrlCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(BaseUrl newBaseUrl) =>
            await _baseUrlCollection.InsertOneAsync(newBaseUrl);

        public async Task UpdateAsync(string id, BaseUrl updatedBaseUrl) =>
            await _baseUrlCollection.ReplaceOneAsync(x => x.Id == id, updatedBaseUrl);

        public async Task RemoveAsync(string id) =>
            await _baseUrlCollection.DeleteOneAsync(x => x.Id == id);
    }
}
