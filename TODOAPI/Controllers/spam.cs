using spamApi.Models;
using SpamApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class SpamController : ControllerBase
    {
        private readonly SpamService _spamService;

        public SpamController(SpamService spamService) =>
            _spamService = spamService;

        [HttpGet]
        public async Task<List<Spam>> Get() =>
            await _spamService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Spam>> Get(string id)
        {
            var spam = await _spamService.GetAsync(id);

            if (spam is null)
            {
                return NotFound();
            }

            return spam;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Spam newSpam)
        {
            await _spamService.CreateAsync(newSpam);
            return CreatedAtAction(nameof(Get), new { id = newSpam.Id }, newSpam);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Spam updatedSpam)
        {
            var spam = await _spamService.GetAsync(id);

            Console.WriteLine("aaaaaaaaaaa");

            if (spam is null)
            {
                return NotFound();
            }

            updatedSpam.Id = spam.Id;
            await _spamService.UpdateAsync(id, updatedSpam);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var spam = await _spamService.GetAsync(id);

            if (spam is null)
            {
                return NotFound();
            }

            await _spamService.RemoveAsync(id);
            return NoContent();
        }
    }
}
