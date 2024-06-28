using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrlsApi.Services;
using urlsApi.Models;
using KoogleDatabaseSettingsApi.Models;
using Microsoft.Extensions.Options;

namespace pageRank
{
    public class PageRank
    {
        private readonly UrlsService _urlsService;
        private Dictionary<string, double> ranks;

        private const double DampingFactor = 0.85; // 
        private const int numofitrtion = 2; 
        private const double Tolerance = 0.0001; 

        public PageRank(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            _urlsService = new UrlsService(databaseSettings);
            ranks = new Dictionary<string, double>();
        }


        public async Task upfutherUrl1(Dictionary<string, Dictionary<string, int>> father1)
{
    Console.WriteLine(8888);
    var a1 = await _urlsService.GetAsync();

    foreach (var fathers in a1)
    {
        foreach (var child in fathers.Children)
        {
            if (!father1.ContainsKey(child.Url))
            {
                father1[child.Url] = new Dictionary<string, int>(); 
            }

            father1[child.Url][fathers.Name] = 1;
            Console.WriteLine(child.Url);
        }
    };
    Console.WriteLine(8888);

    foreach (var key in father1.Keys)
    {
        var value = father1[key];
        await _urlsService.UpdateFieldAsync(key, "Father", value);
    }
}




        public async Task Calculate()
        {
            var urls = await _urlsService.GetAsync();
            int numofallirls = urls.Count;
            if (numofallirls == 0) return;

            foreach (var url in urls)
            {
                ranks[url.Name] = 1.0 / numofallirls;
            }

            bool converged = false;
            for (int iteration = 0; iteration < numofitrtion && !converged; iteration++)
            {
                var newRanks = new Dictionary<string, double>();
                foreach (var url in urls)
                {
                    double sum = 0.0;
                    foreach (var fatherKey in url.Father?.Keys ?? Enumerable.Empty<string>())

                    {
                        var fatherUrl = await _urlsService.GeturlAsync(fatherKey);
                        if (fatherUrl != null && fatherUrl.Children?.Count > 0)
                        {
                            sum += (ranks[fatherUrl.Name] / fatherUrl.Children.Count);
                        }
                    }
                    double newRank = (1 - DampingFactor) / numofallirls + DampingFactor * sum;
                    newRanks[url.Name] = newRank;
                }

                converged = true;
                foreach (var url in urls)
                {
                    if (Math.Abs(newRanks[url.Name] - ranks[url.Name]) > Tolerance)
                    {
                        converged = false;
                        break;
                    }
                }

                ranks = newRanks;

                foreach (var url in urls)
                {
                    await _urlsService.UpdateRankAsync(url.Id, ranks[url.Name]);
                }
            }
        }
    }
}
