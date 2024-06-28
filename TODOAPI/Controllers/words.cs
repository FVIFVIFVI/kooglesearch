using wordsApi.Models;
using WordsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;

namespace WordsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class WordsController : ControllerBase
    {
        private readonly WordsService _wordsService;

        public WordsController(WordsService wordsService) =>
            _wordsService = wordsService;

        [HttpGet("{name}")]
        public async Task<List<Tuple<string, int>>> Get(string name)
        {
            var urlsData = new Dictionary<string, int>();
            var allwords = new Dictionary<string, int>();
            var omit = new HashSet<string>();
            List<Tuple<string, int>> stringList = new List<Tuple<string, int>>();
            var listwords = name.Split(new[] { '\r', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int name1 = listwords.Length;

            foreach (var word in listwords)
            {
                var excludedUrls = new HashSet<string>();

                var tempword = word;
                var list_pointer = new List<HashSet<string>>();
                list_pointer.Add(null);
                list_pointer[0] = excludedUrls;
                Console.WriteLine(name1);
                if (word.StartsWith("-"))
                {
                    tempword = word.Substring(1);
                    list_pointer[0] = omit;
                    name1= name1-1;
                    Console.WriteLine(name1);

                }
                
                var words = await _wordsService.GetAsync(tempword);
                
                    if (words == null || words.Dict == null)
                {
                    continue;
                }

                foreach (var result in words.Dict)
                {
                    

                    //if (word.StartsWith("-"))
                    //{
                    //    omit.Add(result.Url);
                    //    tempword = word.Substring(1);
                    //    continue;
                    //}

                    if (excludedUrls.Contains(result.Url) ) continue;
                    list_pointer[0].Add(result.Url);
                    if (list_pointer[0] == omit) continue;
                    excludedUrls.Add(result.Url);
                    if (!allwords.ContainsKey(result.Url))
                    {
                        allwords[result.Url] = 0;
                        urlsData[result.Url] = 0;
                    }
                    allwords[result.Url]++;
                    urlsData[result.Url] += result.Count;
                }
            }

            foreach (var url in allwords.Keys.ToList())
            {
                if (allwords[url] < name1)
                {
                    Console.WriteLine(allwords[url]+ $"{ listwords.Length}");
                    allwords.Remove(url);
                }
            }
            foreach(var i in omit)
            {
               Console.WriteLine(i);
                urlsData[i] = 0;
            }

            //foreach (var url in urlsData.Keys.ToList()) { Console.WriteLine(urlsData[url]); }

            foreach (var word1 in allwords.Keys)
            {
                stringList.Add(new Tuple<string, int>(word1, urlsData[word1] / name1));
                Console.WriteLine(word1+","+ urlsData[word1]+"+"+ name1);
            }
            Console.WriteLine("kldp0000");
            var sortedList = stringList.OrderByDescending(tuple => tuple.Item2).Take(10).ToList();
            return sortedList;
        }



       

        [HttpPost]
        public async Task<IActionResult> Post(Words newWords)
        {
            await _wordsService.CreateAsync(newWords);
            return CreatedAtAction(nameof(Get), new { id = newWords.Id }, newWords);
        }

        [HttpPut]
        public async Task<IActionResult> Update( Words updatedWords)
        {

            Console.WriteLine("ddddddd");
       
            await _wordsService.CreateAsync(updatedWords);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var words = await _wordsService.GetAsync(id);

            if (words is null)
            {
                return NotFound();
            }

            await _wordsService.RemoveAsync(id);
            return NoContent();
        }
    }
}
