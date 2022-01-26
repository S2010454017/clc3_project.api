using CLC3_Project.Model;
using CLC3_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLC3_Project.Controllers
{
    /// <summary>
    /// Handles all REST-Request concerning the book site.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService) =>
            _booksService = booksService;

        /// <summary>
        /// Get all books in db.
        /// </summary>
        /// <returns>List containing all books</returns>
        [HttpGet]
        public async Task<List<Book>> Get() =>
            await _booksService.GetAsync();

        /// <summary>
        /// Returns the book with the given isbn. If a book is not found
        /// with this isbn, the azure function will be triggered to get the data.
        /// </summary>
        /// <param name="id">isbn of the book</param>
        /// <returns>The book with the isbn</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _booksService.GetAsync(id, true);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }

        /// <summary>
        /// Get all books belonging the a given category.
        /// </summary>
        /// <param name="c">category</param>
        /// <returns>List of books belonging to the category</returns>
        [HttpGet]
        [Route("/category")]
        public async Task<List<Book>> GetForCat(string c) => 
            await _booksService.GetByCategory(c);
  

        /// <summary>
        /// Saves a new book into the db.
        /// </summary>
        /// <param name="newBook">The new book</param>
        /// <returns>The url where the book can be obtained</returns>
        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _booksService.CreateAsync(newBook);

            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }

        /// <summary>
        /// Updates a book in the db.
        /// </summary>
        /// <param name="id">isbn of the to update book</param>
        /// <param name="updatedBook">the new Book</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Book updatedBook)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            updatedBook.Id = book.Id;
            updatedBook.ISBN = book.ISBN;

            await _booksService.UpdateAsync(id, updatedBook);

            return NoContent();
        }

        /// <summary>
        /// Deletes a book with the given isbn.
        /// </summary>
        /// <param name="id">isbn of the book to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _booksService.RemoveAsync(book.Id);

            return NoContent();
        }
    }
}
