using wordsApi.Models;
using WordsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
            //Console.WriteLine($"{name}");
            var urlsData = new Dictionary<string, int>();
            var url1 = new Dictionary<string, int>();
            
            List<Tuple<string, int>> stringList = new List<Tuple<string, int>>();
           // Console.WriteLine(8883);
            var listwords = name.Split(new[] { '\r', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //Console.WriteLine(8885);
            
            foreach (var i1 in listwords)
            {
                //Console.WriteLine(88857);
                //Console.WriteLine(i1);
                var excludedUrls = new HashSet<string>();
                var words = await _wordsService.GetAsync(i1);
                if (words == null || words.Dict == null)
                {
                    //Console.WriteLine($"No data found for {i1}");
                    continue;
                }
                //Console.WriteLine("56565656");

                foreach (var result in words.Dict)
                {
                    Console.WriteLine(result.Url + "9");
                    if (excludedUrls.Contains(result.Url))
                    {
                        continue;
                    }
                    else
                    {
                        excludedUrls.Add(result.Url);
                    }

                    //Console.WriteLine(12);
                    if (!url1.ContainsKey(result.Url))
                    {
                        url1[result.Url] = 0;
                        urlsData[result.Url] = 0;
                    }
                    url1[result.Url]++;
                    urlsData[result.Url] += result.Count;
                }
            }
            Console.WriteLine("343434444");

            foreach (var url in url1.Keys.ToList())
            {
                Console.WriteLine(232);
                Console.WriteLine(url);
                Console.WriteLine(url1[url]);
                Console.WriteLine(listwords.Length);
                if (url1[url] < listwords.Length)
                {
                    url1.Remove(url);
                    //urlsData.Remove(url);
                }
            }

            Console.WriteLine(name, "ggggggggg");

            int name1 = listwords.Length;

            foreach (var word1 in url1.Keys)
            {
                stringList.Add(new Tuple<string, int>(word1, urlsData[word1] / name1));
            }

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
