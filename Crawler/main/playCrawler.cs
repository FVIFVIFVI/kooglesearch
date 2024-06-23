using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using baseUrlApi.Models;
using BaseUrlApi.Services;
using CrawlerManager;
using Microsoft.Extensions.Options;
using KoogleDatabaseSettingsApi.Models;
using upserver;
using urlsApi.Models;
using UrlsApi.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace playCrawler
{
    class PlayCrawler
    {
        private UrlsService _urlsService;

        //private BaseUrlService _baseUrlService;
        private IOptions<KoogleDatabaseSettings> _databaseSettings;
        private Upserver Up;

        public PlayCrawler(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;
            //_baseUrlService = new BaseUrlService(databaseSettings);
            _urlsService = new UrlsService(databaseSettings);
            Up = new(databaseSettings);
        }


        public async Task plaing()
        {
            var baseUrl = await _urlsService.GetAsync();
            //var baseUrlList = await Up.GetBaseUrl();
            Random rand = new Random();
            int rand1 = rand.Next(1, 1000);
            string url = null;
            int it = 0;
            foreach (var indexer in baseUrl)
            {
                if (rand1 == it)
                {
                    url = indexer.Name;
                    break;
                }
                it++;
            }
            //foreach (var baseUrl in baseUrlList) {
            
                //Console.WriteLine(url);
                Crawler cr = new Crawler(_databaseSettings);
                cr.crawler(url);
            //}
        }
    }
}