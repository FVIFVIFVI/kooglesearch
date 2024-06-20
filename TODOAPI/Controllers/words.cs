using wordsApi.Models;
using WordsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class WordsController : ControllerBase
    {
        private readonly WordsService _wordsService;

        public WordsController(WordsService wordsService) =>
            _wordsService = wordsService;

        [HttpGet]
        public async Task<List<Words>> Get() {
         Console.WriteLine("ddddddddddddd");
           return await _wordsService.GetAsync();}
            

        [HttpGet("{name}")]
        public async Task<List<Tuple<string, int>>> Get(string name)

        {


            var words = await _wordsService.GetAsync(name);

                     Console.WriteLine(name,"ggggggggg");





            List<Tuple<string, int>> stringList = new List<Tuple<string, int>>();

            List<int> countList = new List<int>();
            

            if (words is null)
            {
                return stringList;
            }

            int sizeoflist = words.Dict?.Count ?? 0;
            foreach (var i in words.Dict)
        
                { 
                     stringList.Add(new Tuple<string, int>(i.Url, i.Count));
                    
                     

                }




            var sortedList = stringList.OrderByDescending(tuple => tuple.Item2).ToList();

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
