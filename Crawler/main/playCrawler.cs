using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using baseUrlApi.Models;
using BaseUrlApi.Services;
using CrawlerManager;
using Microsoft.Extensions.Options;
using KoogleDatabaseSettingsApi.Models;
using upserver;


namespace playCrawler
{
    class PlayCrawler
    {
        private BaseUrlService _baseUrlService;
        private IOptions<KoogleDatabaseSettings> _databaseSettings;
        private Upserver Up;

        public PlayCrawler(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;
            _baseUrlService = new BaseUrlService(databaseSettings);
            Up = new(databaseSettings);
        }


        public async Task plaing()
        {
            var baseUrlList = await Up.GetBaseUrl();

            foreach (var baseUrl in baseUrlList) {
                string url = baseUrl.Url;
                //Console.WriteLine(url);
                Crawler cr = new Crawler(_databaseSettings);
                cr.crawler(url);
            }
        }
    }
}