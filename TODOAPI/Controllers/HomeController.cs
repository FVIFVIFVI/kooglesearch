using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordsApi.Services;
using wordsApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly WordsService _wordsService;

        public HomeController(WordsService wordsService)
        {
            _wordsService = wordsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Home Controller is working");
        }

        [HttpGet("trigger-get-async")]
        public async Task<IActionResult> TriggerGetAsync()
        {
            var hs = new HashSet<string>();
            var newdict = new List<Dict>();
            var wordsList = await _wordsService.GetAsync();

            foreach (var item in wordsList)
            {
                foreach (var item2 in item.Dict)
                {
                    if (!hs.Contains(item2.Url))
                    {
                        hs.Add(item2.Url);
                        newdict.Add(item2);
                    }
                }
                // Update the document with the new Dict list
                await _wordsService.RemoveAndUpdateAsync1(item.Id, newdict);
                // Clear the newdict for the next iteration
                newdict.Clear();
            }

            return Ok("GetAsync function processed successfully");
        }
    }
}
