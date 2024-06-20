using usersApi.Models;
using UsersApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService) =>
            _usersService = usersService;

        [HttpGet]
        public async Task<List<Users>> Get() =>
            await _usersService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Users>> Get(string id)
        {
            var users = await _usersService.GetAsync(id);

            if (users is null)
            {
                return NotFound();
            }

            return users;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Users newUsers)
        {
            await _usersService.CreateAsync(newUsers);
            return CreatedAtAction(nameof(Get), new { id = newUsers.Id }, newUsers);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Users updatedUsers)
        {
            var users = await _usersService.GetAsync(id);

            if (users is null)
            {
                return NotFound();
            }

            updatedUsers.Id = users.Id;
            await _usersService.UpdateAsync(id, updatedUsers);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var users = await _usersService.GetAsync(id);

            if (users is null)
            {
                return NotFound();
            }

            await _usersService.RemoveAsync(id);
            return NoContent();
        }
    }
}
