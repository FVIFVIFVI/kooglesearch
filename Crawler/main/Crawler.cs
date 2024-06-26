﻿using downloadHtml;
using parserUrl;
using parserWords;
using Substring;
using upserver;
using KoogleDatabaseSettingsApi.Models;
using Microsoft.Extensions.Options;
using urlsApi.Models;
using wordsApi.Models;
using ignore.Models;
namespace CrawlerManager;

class Crawler
{
    private Queue<string> links;
    private HashSet<string> visitedLinks;
    private HashSet<string> indata;
    private GetHtml Html;
    private ParserUrl PUrl;
    private Upserver Up;
    private ParserWords PWord;
    private Ignore ignore;
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
    }

    public async void crawler(string Url)
    {
        string prefix = SubString.SubStr(Url, '/', 0, 3);

        links.Enqueue(Url);
        //var UrlList = await Up.GetUrls();
        //foreach (var urlData in UrlList)
        //{
        //    indata.Add(urlData.Name);
        //}

        int count = 0;
        int errors = 0;
        int visit = 0;
        int lin = 0;
        Console.WriteLine("urueu");
        var ignore1=  Up.Ignore_get();
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
                    string htmlContent = Html.getHtmlFromUrl(link);
                    Console.WriteLine(22 );
                    List<DictUrl> urls = PUrl.getUrls(htmlContent, prefix, links);
                    Console.WriteLine(23);
                    if (!indata.Contains(link))
                    {
                        Console.WriteLine(24);
                        //await Up.InsertUrl(urls, link);
                        Dictionary<string, int> words = PWord.getWords(link);
                        foreach (var word in words)
                        {
                            DictWord dWord = new DictWord { Url = link, Count = word.Value };
                            await Up.InsertOrUpdateWord(dWord, word.Key);
                        }
                    }
                    //Console.WriteLine(link);
                    lin++;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Error: {ex.Message}");
                    
                    ++errors;
                }
                visitedLinks.Add(link);
                await Up.InsertIgnore(link);
                Up.UpdateAsync1(links.Peek());


            }
            else
            {   Console.WriteLine(links.Peek());
                links.Dequeue();
                Console.WriteLine("y7yughjvjfytt6itgugvyg");
                visit++;
            }
        }
        //Console.WriteLine($"errors:{errors}, visit:{visit}, link:{lin}");
    }
}