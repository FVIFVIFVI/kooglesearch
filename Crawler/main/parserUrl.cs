using HtmlAgilityPack;
using Substring;
using urlsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace parserUrl
{
    class ParserUrl
    {

        private HtmlDocument _Html;

        public ParserUrl()
        {
            _Html = new();
        }

        private static string CombineLink(string prefix, string href)
        {
            string pre = SubString.SubStr(prefix, ':', 1, 2);

            if (href.StartsWith(pre, StringComparison.OrdinalIgnoreCase)) {
                return "https:" + href;
            } else if (!href.StartsWith("http", StringComparison.OrdinalIgnoreCase) && href != "") {
                return prefix + href;
            } else if (href.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) {
                return href;
            }
            return "";
        }

        public List<DictUrl> getUrls(string html, string prefix, Queue<string> links)
        {
            List<DictUrl> resultDict = new List<DictUrl>();
            _Html.LoadHtml(html);

            var linkNodes = _Html.DocumentNode.SelectNodes("//a[@href]");
            if (linkNodes != null) {
                foreach (var linkNode in linkNodes) {
                    string href = linkNode.GetAttributeValue("href", "");
                    
                    string link = CombineLink(prefix, href);
                    //Console.WriteLine(link);
                    if (link != "" && Uri.IsWellFormedUriString(link, UriKind.Absolute)) {
                        links.Enqueue(link);
                        

                        var existingDict = resultDict.FirstOrDefault(dict => string.Equals(dict.Url, link, StringComparison.OrdinalIgnoreCase));
                        if (existingDict != null) {
                            existingDict.Count += 1;
                        } else {
                            resultDict.Add(new DictUrl { Url = link, Count = 1 });
                        }
                    }
                }
            }
            return resultDict;
        }
    }
}