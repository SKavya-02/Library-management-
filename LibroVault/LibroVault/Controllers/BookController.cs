using Library.API.DTO;
using Library.API.Interfaces;
using Library.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBook([FromBody] BookDto request)
        {
            try
            {
                var existing = await _service.GetBookByISBNAsync(request.ISBN);
                if (existing != null)
                    return BadRequest("Book with this ISBN already exists.");

                var book = new Book
                {
                    Title = request.Title,
                    Author = request.Author,
                    ISBN = request.ISBN,
                    Publisher = request.Publisher,
                    PublicationDate = request.PublicationDate,
                    Edition = request.Edition,
                    Language = request.Language,
                    NumberOfPages = request.NumberOfPages,
                    Description = request.Description,
                    Cost = request.Cost,
                    ImageUrl = request.ImageUrl,
                    CategoryId = request.CategoryId
                };

                await _service.AddBookAsync(book);
                return Ok("Book added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to add book: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBooks([FromQuery] string? search = "", [FromQuery] int? categoryId = null)
        {
            try
            {
                var books = await _service.GetAllBooksAsync(search, categoryId);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to fetch books: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto request)
        {
            try
            {
                var book = await _service.GetBookByIdAsync(id);
                if (book == null)
                    return NotFound("Book not found.");

                book.Title = request.Title;
                book.Author = request.Author;
                book.ISBN = request.ISBN;
                book.Publisher = request.Publisher;
                book.PublicationDate = request.PublicationDate;
                book.Edition = request.Edition;
                book.Language = request.Language;
                book.NumberOfPages = request.NumberOfPages;
                book.Description = request.Description;
                book.Cost = request.Cost;
                book.ImageUrl = request.ImageUrl;
                book.CategoryId = request.CategoryId;

                await _service.UpdateBookAsync(book);
                return Ok("Book updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update book: {ex.Message}");
            }
        }

        [HttpPut("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteBook(int id)
        {
            try
            {
                var book = await _service.GetBookByIdAsync(id);
                if (book == null)
                    return NotFound("Book not found.");

                await _service.SoftDeleteBookAsync(book);
                return Ok("Book soft-deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to soft-delete book: {ex.Message}");
            }
        }
    }
}
