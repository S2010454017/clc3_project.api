using CLC3_Project.Model;
using CLC3_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLC3_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadListController : Controller
    {
        private readonly ReadListService _readListService;

        public ReadListController(ReadListService readListService)
        {
            _readListService = readListService;
        }

        [HttpGet]
        public async Task<List<ReadingList>> Get([FromHeader] string owner) =>
           await _readListService.GetListForOwnerAsync(owner);

        [HttpGet("{name}")]
        public async Task<ActionResult<ReadingList>> Get(string name, [FromHeader] string owner)
        {
            var list = await _readListService.GetAsync(name, owner);

            if (list is null)
            {
                return NotFound();
            }

            return list;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ReadingList newlist, [FromHeader] string user)
        {
            await _readListService.CreateAsync(newlist);

            return CreatedAtAction(nameof(Get), new { id = newlist.Id }, newlist);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Update(string name, [FromHeader] string user, ReadingList updatedlist)
        {
            var list = await _readListService.GetAsync(name, user);

            if (list is null)
            {
                return NotFound();
            }

            updatedlist.Id = list.Id;
            updatedlist.Owner = list.Owner;

            await _readListService.UpdateAsync(updatedlist);

            return NoContent();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name, [FromHeader] string user)
        {
            var list = await _readListService.GetAsync(name, user);

            if (list is null)
            {
                return NotFound();
            }

            await _readListService.RemoveAsync(list.Id);

            return NoContent();
        }

    }
}
