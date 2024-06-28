using Microsoft.Extensions.Options;
using MongoDB.Driver;
using wordsApi.Models;
using KoogleDatabaseSettingsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsApi.Services
{
    public class WordsService
    {
        private readonly IMongoCollection<Words> _wordsCollection;

        public WordsService(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _wordsCollection = database.GetCollection<Words>(databaseSettings.Value.WordsCollectionName);
        }

        public async Task<List<Words>> GetAsync() =>
            await _wordsCollection.Find(_ => true).ToListAsync();

        public async Task<Words?> GetAsync(string id)
        {
            Console.WriteLine(id);
            return await _wordsCollection.Find(x => x.Name == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Words newWords) =>
            await _wordsCollection.InsertOneAsync(newWords);

        public async Task UpdateAsync(string id, Words updatedWords) =>
            await _wordsCollection.ReplaceOneAsync(x => x.Id == id, updatedWords);

        public async Task RemoveAsync(string id) =>
            await _wordsCollection.DeleteOneAsync(x => x.Id == id);

        public async Task RemoveAndUpdateAsync1(string id, List<Dict> newDictList)
        {
            Console.WriteLine($"Starting to update document with ID: {id}");

            var filter = Builders<Words>.Filter.Eq(w => w.Id, id);
            var update = Builders<Words>.Update.Set(w => w.Dict, newDictList);

            try
            {
                var result = await _wordsCollection.UpdateOneAsync(filter, update);

                if (result.ModifiedCount > 0)
                {
                    Console.WriteLine($"Successfully updated document with ID: {id}");
                }
                else
                {
                    Console.WriteLine($"No document found with ID: {id}, or no changes were made.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the document with ID: {id}. Error: {ex.Message}");
            }
        }

    }
}
