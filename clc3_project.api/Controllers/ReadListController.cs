using CLC3_Project.Model;
using CLC3_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLC3_Project.Controllers
{
    /// <summary>
    /// Handles all REST-Request concerning the Readinglist side.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReadListController : Controller
    {
        private readonly ReadListService _readListService;

        public ReadListController(ReadListService readListService)
        {
            _readListService = readListService;
        }

        /// <summary>
        /// Get all lists for the user.
        /// </summary>
        /// <param name="owner">username</param>
        /// <returns>The list containing all reading lists of the user</returns>
        [HttpGet]
        public async Task<List<ReadingList>> Get([FromHeader] string owner) =>
           await _readListService.GetListForOwnerAsync(owner);

        /// <summary>
        /// Get a specific reading list for a given user.
        /// </summary>
        /// <param name="name">name of the desired list</param>
        /// <param name="owner">username</param>
        /// <returns>The reading list.</returns>
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

        /// <summary>
        /// Saves a new reading list for the given user into the db.
        /// </summary>
        /// <param name="newlist">the reading list to created</param>
        /// <param name="user">username</param>
        /// <returns>URL where the reading list can be accessed</returns>
        [HttpPost]
        public async Task<IActionResult> Post(ReadingList newlist, [FromHeader] string user)
        {
            await _readListService.CreateAsync(newlist);

            return CreatedAtAction(nameof(Get), new { id = newlist.Id }, newlist);
        }

        /// <summary>
        /// Updates a reading list in the db.
        /// </summary>
        /// <param name="name">name of the reading list to update</param>
        /// <param name="user">username</param>
        /// <param name="updatedlist">new data for the readinglist</param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a reading list for a user from the db.
        /// </summary>
        /// <param name="name">name of list to delete.</param>
        /// <param name="user">username</param>
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
