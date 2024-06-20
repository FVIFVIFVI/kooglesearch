using Microsoft.Extensions.Options;
using MongoDB.Driver;
using urlsApi.Models;
using MongoDB.Bson;

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

        public async Task<Urls?> GeturlAsync(string id)
        {
            try
            {
                Console.WriteLine("Attempting to retrieve URL: " + id);
                return await _urlsCollection.Find(x => x.Name == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving URL: " + ex.Message);
                return null;
            }
        }


        public async Task DeleteIfFieldNullAsync(string fieldName)
        {
            try
            {
                var filter = Builders<Urls>.Filter.Eq(fieldName, BsonNull.Value);

                var result = await _urlsCollection.DeleteManyAsync(filter);

                Console.WriteLine($"Deleted {result.DeletedCount} documents where '{fieldName}' was null.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting documents where '{fieldName}' is null: {ex.Message}");
            }
        }



        public async Task CreateAsync(Urls newUrls) =>
            await _urlsCollection.InsertOneAsync(newUrls);

        public async Task UpdateAsync(string id, Urls updatedUrls) =>
            await _urlsCollection.ReplaceOneAsync(x => x.Id == id, updatedUrls);


        public async Task UpdateFieldAsync<TField>(string id, string fieldName, TField newValue)
        {
            Console.WriteLine("hhh5");
            var filter = Builders<Urls>.Filter.Eq(x => x.Name, id);
            var update = Builders<Urls>.Update.Set(fieldName, newValue);

            await _urlsCollection.UpdateOneAsync(filter, update);
        }
          


          public async Task UpdateRankAsync(string id, double newRank)
        {
            var filter = Builders<Urls>.Filter.Eq(x => x.Id, id);
            var update = Builders<Urls>.Update.Set(x => x.Rank, newRank);

            await _urlsCollection.UpdateOneAsync(filter, update);
        }
       
        public async Task RemoveAsync(string id) =>
            await _urlsCollection.DeleteOneAsync(x => x.Id == id);

       

    }



}
