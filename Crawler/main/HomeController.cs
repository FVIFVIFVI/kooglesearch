using Microsoft.AspNetCore.Mvc;
using upserver;
using urlsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ignore.Models;
using downloadHtml;
using parserUrl;
using Substring;
using pageRank;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;

namespace TodoApi.main
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly Upserver _upServer;
        private readonly Dictionary<string, Dictionary<string, int>> _father2;
        private readonly Ignore _ignore;
        private readonly GetHtml _html;
        private readonly ParserUrl _pUrl;
        private readonly PageRank _pageRank; // Add PageRank as a dependency

        public HomeController(Upserver upServer, PageRank pageRank)
        {
            _upServer = upServer;
            _ignore = new Ignore();
            _html = new GetHtml();
            _pUrl = new ParserUrl();
            _father2 = new Dictionary<string, Dictionary<string, int>>();
            _pageRank = pageRank; // Initialize PageRank
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                Console.WriteLine("Index");
                var allUrls = await _upServer.getallurl();
                foreach (var url in allUrls)
                {
                    foreach (var item in url.Children)
                    {
                        if (!_father2.ContainsKey(item.Url))
                        {
                            _father2[item.Url] = new Dictionary<string, int>();
                        }
                        _father2[item.Url][url.Name] = 1;
                    }
                }

                foreach (var fatherKey in _father2.Keys)
                {
                    var tempDict = _father2[fatherKey];
                    await _upServer.upfutherUrl1(tempDict, fatherKey);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred in Index: " + ex.Message);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUrl()
        {
            try
            {
                Console.WriteLine("HttpPost");
                var ignoreList = _upServer.Ignore_get();
                foreach (var urlPast in ignoreList)
                {
                    string prefix = SubString.SubStr(urlPast, '/', 0, 3);
                    string htmlContent = _html.getHtmlFromUrl(urlPast);
                    Console.WriteLine(22);

                    // Create a queue with the current URL
                    Queue<string> links = new Queue<string>();
                    links.Enqueue(urlPast);

                    List<DictUrl> urls = _pUrl.getUrls(htmlContent, prefix, links);
                    while (links.Count > 0)
                    {
                        if (!_father2.ContainsKey(urlPast))
                        {
                            _father2[urlPast] = new Dictionary<string, int>();
                        }

                        _father2[urlPast][links.Dequeue()] = 1;
                    }

                    // Update the InsertUrl call to include all necessary parameters
                    await _upServer.InsertUrl(urls, urlPast, new Dictionary<string, int>());
                }

                // Correct iteration and updating of further URLs
                foreach (var element in _father2)
                {
                    var fatherUrl = element.Key;
                    var tempDict = element.Value;
                    await _upServer.upfutherUrl1(tempDict, fatherUrl);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred in AddUrl: " + ex.Message);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost("calculate-pagerank")] // Define a new route for the Calculate method
        public async Task<IActionResult> CalculatePageRank()
        {
            try
            {
                Console.WriteLine("Calculating PageRank");
                await _pageRank.Calculate1();
                return Ok("PageRank calculation completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred in CalculatePageRank: " + ex.Message);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
