using downloadHtml;
using parserUrl;
using parserWords;
using Substring;
using upserver;
using KoogleDatabaseSettingsApi.Models;
using Microsoft.Extensions.Options;
using urlsApi.Models;
using wordsApi.Models;
using ignore.Models;
using pageRank;
using System.Runtime.ExceptionServices;

namespace CrawlerManager;

class Crawler
{
    private PageRank pageRank1;
    private Queue<string> links;
    private HashSet<string> visitedLinks;
    private HashSet<string> indata;
    private GetHtml Html;
    private ParserUrl PUrl;
    private Upserver Up;
    private ParserWords PWord;
    private Ignore ignore;
    bool first = true;

    public Crawler(IOptions<KoogleDatabaseSettings> databaseSettings)
    {
        links = new();
        visitedLinks = new();
        indata = new();
        Html = new();
        PUrl = new();
        Up = new(databaseSettings);
        PWord = new();
        ignore = new();
        pageRank1 = new(databaseSettings);
        
    }

    public List<DictUrl> parserurl1(string link, string prefix)
    {
        string htmlContent = Html.getHtmlFromUrl(link);
        Console.WriteLine(22);
        List<DictUrl> urls = PUrl.getUrls(htmlContent, prefix, links);
        return urls;
    }

    public async void crawler(string Url)
    {
        string prefix = SubString.SubStr(Url, '/', 0, 3);
        links.Enqueue(Url);
        await Up.UpdateAsync1(Url);
        int count = 0;
        int errors = 0;
        int visit = 0;
        int lin = 0;
        Console.WriteLine("urueu");
        var ignore1 = Up.Ignore_get();
        foreach (var item in ignore1) { visitedLinks.Add(item); }

        while (links.Count > 0)
        {
            ++count;
            Console.Write($"{count}, ");
            string link = links.Dequeue();
            if (!visitedLinks.Contains(link))
            {
                try
                {
                    List<DictUrl> urls = parserurl1(link, prefix);
                    Console.WriteLine(23);
                    if (!indata.Contains(link))
                    {
                        Console.WriteLine(24);
                        Dictionary<string, int> words = PWord.getWords(link);
                        foreach (var word in words)
                        {
                            DictWord dWord = new DictWord { Url = link, Count = word.Value };
                            await Up.InsertOrUpdateWord(dWord, word.Key);
                        }
                    }
                    lin++;
                }
                catch (Exception ex)
                {
                    ++errors;
                }
                visitedLinks.Add(link);
                await Up.InsertIgnore(link);
                Up.UpdateAsync1(links.Peek());
            }
            else
            {   if (!first) { return; }
                parserurl1(link, prefix);
                Console.WriteLine(links.Peek());
                links.Dequeue();
                Console.WriteLine("y7yughjvjfytt6itgugvyg");
                first= false;
                visit++;
            }
        }
    }
}
