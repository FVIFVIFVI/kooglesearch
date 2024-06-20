using urlsApi.Models;
using UrlsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UrlsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class UrlsController : ControllerBase
    {
        private readonly UrlsService _urlsService;

        public UrlsController(UrlsService urlsService) =>
            _urlsService = urlsService;

        [HttpGet]
        public async Task<List<Urls>> Get() =>
            await _urlsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Urls>> Get(string id)
        {
            var urls = await _urlsService.GetAsync(id);

            if (urls is null)
            {
                return NotFound();
            }

            return urls;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Urls newUrls)
        {
            await _urlsService.CreateAsync(newUrls);
            return CreatedAtAction(nameof(Get), new { id = newUrls.Id }, newUrls);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Urls updatedUrls)
        {
            var urls = await _urlsService.GetAsync(id);

            if (urls is null)
            {
                return NotFound();
            }

            updatedUrls.Id = urls.Id;
            await _urlsService.UpdateAsync(id, updatedUrls);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var urls = await _urlsService.GetAsync(id);

            if (urls is null)
            {
                return NotFound();
            }

            await _urlsService.RemoveAsync(id);
            return NoContent();
        }
    }
}
