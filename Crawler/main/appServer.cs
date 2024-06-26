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
using ignore.Models;
using ignore.Services;
using System.Security.Cryptography.X509Certificates;

namespace upserver
{
    class Upserver
    {
        private BaseUrlService _baseUrlService;
        private UrlsService _urlsService;
        private WordsService _wordsService;
        private IgnoreService _ignoreService;
        public List<Ignore> IgnoreList { get; private set; }

        public Upserver(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            _baseUrlService = new BaseUrlService(databaseSettings);
            _urlsService = new UrlsService(databaseSettings);
            _wordsService = new WordsService(databaseSettings);
            _ignoreService = new IgnoreService(databaseSettings);
            InitializeAsync().GetAwaiter().GetResult();
        }

    private async Task InitializeAsync()
    {
        IgnoreList = await _ignoreService.GetAsync();
    }
    public HashSet<string> Ignore_get()
        {
            try
            {
                var ignoreList = IgnoreList;
                var combinedHashSet = new HashSet<string>();

                foreach (var ignore1 in ignoreList)
                {
                    foreach (var ignore2 in ignore1.visited)
                    {
                        combinedHashSet.Add(ignore2);
                    }
                }

                return combinedHashSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to retrieve Ignore documents. Error: " + ex.Message);
                return new HashSet<string>();
            }
        }

        public async Task InsertIgnore(string urlvisit)
        {
            try
            {
                if (IgnoreList == null || IgnoreList.Count == 0)
                {
                    var newIgnore = new Ignore();
                    newIgnore.visited.Add(urlvisit);
                    await _ignoreService.CreateAsync(newIgnore);
                    IgnoreList = await _ignoreService.GetAsync(); // עדכון הרשימה לאחר ההוספה
                }
                else
                {
                    await _ignoreService.AppendToVisitedAsync(IgnoreList[0].Id, urlvisit);
                }

                Console.WriteLine("InsertIgnore succeeded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("InsertIgnore failed. Error: " + ex.Message);
            }
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
            catch (Exception ex)
            {
                Console.WriteLine("InsertUrl failed for: " + startlink + ". Error: " + ex.Message);
            }
        }

        public async Task upfutherUrl(string id, string link1)
        {
            var tempdict = new Dictionary<string, int>();
            var hash = await _urlsService.GeturlAsync(id);
            Console.WriteLine(8888);

            if (true)
            {
                Console.WriteLine("dsds66666666d");
                tempdict = hash.Father;
                tempdict[link1] = 1;
                await _urlsService.UpdateFieldAsync(id, "Father", tempdict);
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

        public async Task UpdateAsync1(string url)
        {
            Console.WriteLine("UpdateAsync1");
            var baseUrl = await _baseUrlService.GetAsync();
            var new_baseurl = new BaseUrl
            {
                Name = baseUrl[0].Name,
                Id = baseUrl[0].Id,
                Time = baseUrl[0].Time,
                Url = url
            };
            await _baseUrlService.UpdateAsync(url, new_baseurl);
        }
    }
}
