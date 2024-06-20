using baseUrlApi.Models;
using BaseUrlApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseUrlApi.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class BaseUrlController : ControllerBase
    {
        private readonly BaseUrlService _baseUrlService;

        public BaseUrlController(BaseUrlService baseUrlService) =>
            _baseUrlService = baseUrlService;

        [HttpGet]
        public async Task<List<BaseUrl>> Get() =>
            await _baseUrlService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<BaseUrl>> Get(string id)
        {
            var baseUrl = await _baseUrlService.GetAsync(id);

            if (baseUrl is null)
            {
                return NotFound();
            }

            return baseUrl;
        }

        [HttpPost]
        public async Task<IActionResult> Post(BaseUrl newBaseUrl)
        {
            await _baseUrlService.CreateAsync(newBaseUrl);
            return CreatedAtAction(nameof(Get), new { id = newBaseUrl.Id }, newBaseUrl);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BaseUrl updatedBaseUrl)
        {
            var baseUrl = await _baseUrlService.GetAsync(id);

            if (baseUrl is null)
            {
                return NotFound();
            }

            updatedBaseUrl.Id = baseUrl.Id;
            await _baseUrlService.UpdateAsync(id, updatedBaseUrl);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var baseUrl = await _baseUrlService.GetAsync(id);

            if (baseUrl is null)
            {
                return NotFound();
            }

            await _baseUrlService.RemoveAsync(id);
            return NoContent();
        }
    }
}
