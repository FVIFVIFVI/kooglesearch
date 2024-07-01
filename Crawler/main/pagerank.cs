using System;
using System.Collections.Generic;
using System.Linq;
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

        private const double DampingFactor = 0.85;
        private const int numOfIterations = 10;
        private const double Tolerance = 0.0001;

        public PageRank(IOptions<KoogleDatabaseSettings> databaseSettings)
        {
            _urlsService = new UrlsService(databaseSettings);
            ranks = new Dictionary<string, double>();
        }

        public async Task UpdateFatherUrlAsync(Dictionary<string, Dictionary<string, int>> fatherUrls)
        {
            var urls = await _urlsService.GetAsync();

            foreach (var url in urls)
            {
                foreach (var child in url.Children)
                {
                    if (!fatherUrls.ContainsKey(child.Url))
                    {
                        fatherUrls[child.Url] = new Dictionary<string, int>();
                    }

                    fatherUrls[child.Url][url.Name] = 1;
                }
            }

            foreach (var key in fatherUrls.Keys)
            {
                var value = fatherUrls[key];
                await _urlsService.UpdateFieldAsync(key, "Father", value);
            }
        }

        public async Task Calculate()
        {
            var urls = await _urlsService.GetAsync();
            int numOfAllUrls = urls.Count;
            if (numOfAllUrls == 0) return;

            foreach (var url in urls)
            {
                ranks[url.Name] = 1.0 / numOfAllUrls;
            }

            bool converged = false;
            for (int iteration = 0; iteration < numOfIterations && !converged; iteration++)
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
                    double newRank = (1 - DampingFactor) / numOfAllUrls + DampingFactor * sum;
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

        public async Task Calculate1()
        {
            var rank1 = new Dictionary<string, double>();
            var existingUrls = new HashSet<string>();
            var numOfChildren = new Dictionary<string, int>();
            var idUrl = new Dictionary<string, string>();
            var ranksLinks = new Dictionary<string, HashSet<string>>();

            var allUrls = await _urlsService.GetAsync();
            foreach (var url in allUrls)
            {
                Console.WriteLine(url.Name);
                existingUrls.Add(url.Name);
                ranksLinks[url.Name] = new HashSet<string>();
                numOfChildren[url.Name] = url.Children.Count;
                rank1[url.Name] = 1.0;  // Initialize ranks
            }

            foreach (var url in allUrls)
            {
                idUrl[url.Name] = url.Id;
                foreach (var parent in url.Father.Keys)
                {
                    Console.WriteLine("url.Name");
                    if (existingUrls.Contains(parent))
                    {
                        ranksLinks[url.Name].Add(parent);
                    }
                }
            }

            for (int iteration = 0; iteration < numOfIterations; iteration++)
            {
                Console.WriteLine("newRanks");
                var newRanks = new Dictionary<string, double>();
                foreach (var url in existingUrls)
                {
                    double sum = 0;
                    foreach (var parent in ranksLinks[url])
                    {
                        sum += rank1[parent] / numOfChildren[parent];
                        Console.WriteLine("sum");
                    }
                    newRanks[url] = (1 - DampingFactor) / existingUrls.Count + DampingFactor * sum;
                }

                rank1 = newRanks;
            }

            foreach (var rankUrl in rank1.Keys)
            {
                Console.WriteLine("{rankUrl}");
                await _urlsService.UpdateRankAsync(idUrl[rankUrl], rank1[rankUrl]);

            }
        }
    }
}
