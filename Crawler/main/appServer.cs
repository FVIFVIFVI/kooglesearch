using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using baseUrlApi.Models;
using urlsApi.Models;
using BaseUrlApi.Services;
using UrlsApi.Services;
using WordsApi.Services;
using Microsoft.Extensions.Options;
using KoogleDatabaseSettingsApi.Models;
using wordsApi.Models;
using MongoDB.Driver;

namespace upserver
{
    class Upserver
    {
        private BaseUrlService _baseUrlService;
        private UrlsService _urlsService;
        private WordsService _wordsService;

        public Upserver(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            _baseUrlService = new BaseUrlService(databaseSettings);
            _urlsService = new UrlsService(databaseSettings);
            _wordsService = new WordsService(databaseSettings);
        }

        public async Task InsertUrl(List<DictUrl> child, string startlink, Dictionary<string, int> father1)
        {
            var newUrls = new Urls
            {
                Name = startlink,
                Time = DateTime.UtcNow.ToString(),
                Children = child,
                Father = father1
            };

            try
            {
                await _urlsService.CreateAsync(newUrls);
                Console.WriteLine("InsertUrl succeeded for: " + startlink); //
            }
            catch (Exception ex) // 
            {
                Console.WriteLine("InsertUrl failed for: " + startlink + ". Error: " + ex.Message); // Failure message
            }
        }



        public async Task upfutherUrl(string id, string link1)
        {
            //Console.WriteLine(id);
            var tempdict = new Dictionary<string, int>();

            var hash = await _urlsService.GeturlAsync(id);
            Console.WriteLine(8888);

            if (1==1){
                Console.WriteLine("dsds66666666d");
                tempdict = hash.Father;
                tempdict[link1]=1;
            await _urlsService.UpdateFieldAsync(id,"Father",tempdict);
           }

        }

        public async Task<List<BaseUrl>> GetBaseUrls()
        {
            return await _baseUrlService.GetAsync();
        }

        public async Task<List<Urls>> GetUrls()
        {
            return await _urlsService.GetAsync();
        }

        public async Task InsertOrUpdateWord(DictWord child, string startlink)
        {
            var exitword = await _wordsService.GetWordAsync(startlink);
            //var excludedUrls = new HashSet<string>();
            //foreach (var i in exitword.Dict)
            //{
            //    if (i.Url == child.Url) { return; }
            //}
            if (exitword != null)
            {
                await updateword(exitword.Id, child);
            }
            else
            {
                var word = new Words
                {
                    Name = startlink,
                    Dict = new List<DictWord> { child }
                };

                await _wordsService.CreateAsync(word);
            }
        }


        public async Task InsertWord(DictWord child, string startlink)
        {
            var word = new Words
            {
                Name = startlink,
                Dict = new List<DictWord> { child }
            };

            await _wordsService.CreateAsync(word);
        }



        public async Task<bool> UrlExistsAsync(string url)
        {
            var urlRecord = await _urlsService.GeturlAsync(url);
            return urlRecord != null;
        }


        public async Task updateword(string id, DictWord newDictItem)
        {
            var filter = Builders<Words>.Filter.Eq(x => x.Id, id);
            var update = Builders<Words>.Update.Push(x => x.Dict, newDictItem);
            await _wordsService.UpdateOneAsync(filter, update);
        }

        public async Task DeleteUrlsWhereFatherIsNullAsync()
        {
            try
            {
                await _urlsService.DeleteIfFieldNullAsync("Father");
                Console.WriteLine("Successfully deleted all URLs where 'Father' is null.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete URLs where 'Father' is null: {ex.Message}");
            }
        }


        public async Task<List<BaseUrl>> GetBaseUrl()
        {
            return await _baseUrlService.GetAsync();
        }
    }
}