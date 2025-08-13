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
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _service;

        public BorrowController(IBorrowService service)
        {
            _service = service;
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowDto request)
        {
            try
            {
                var borrow = new BorrowedBook
                {
                    UserId = request.UserId,
                    BookId = request.BookId,
                    BorrowedDate = request.BorrowedDate,
                    IsReturned = false,
                    IsLost = false,
                    FineAmount = 0
                };

                await _service.AddBorrowAsync(borrow);
                return Ok("Book borrowed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error borrowing book: {ex.Message}");
            }
        }

        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            try
            {
                var borrow = await _service.GetBorrowByIdAsync(id);
                if (borrow == null)
                    return NotFound("Borrow record not found.");

                if (borrow.IsReturned)
                    return BadRequest("Book is already returned.");

                borrow.IsReturned = true;
                borrow.ReturnDate = DateTime.Now;

                var dueDate = borrow.BorrowedDate.AddDays(14);
                var lateDays = (borrow.ReturnDate.Value - dueDate).Days;
                borrow.FineAmount = lateDays > 0 ? lateDays * 10 : 0;

                await _service.UpdateBorrowAsync(borrow);
                return Ok($"Book returned successfully. Fine: ₹{borrow.FineAmount}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error returning book: {ex.Message}");
            }
        }

        [HttpPut("marklost/{id}")]
        public async Task<IActionResult> MarkAsLost(int id)
        {
            try
            {
                var borrow = await _service.GetBorrowByIdAsync(id);
                if (borrow == null)
                    return NotFound("Borrow record not found.");

                if (borrow.IsReturned)
                    return BadRequest("Book is already returned and cannot be marked as lost.");

                if (borrow.IsLost)
                    return BadRequest("Book is already marked as lost.");

                borrow.IsLost = true;
                borrow.FineAmount = 500;

                await _service.UpdateBorrowAsync(borrow);
                return Ok($"Book marked as lost. Fine: ₹{borrow.FineAmount}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error marking book as lost: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBorrowedBooks([FromQuery] int? userId, [FromQuery] int? bookId, [FromQuery] bool? isReturned)
        {
            try
            {
                var result = await _service.GetAllBorrowedBooksAsync(userId, bookId, isReturned);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching borrowed books: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBorrowedBook(int id, [FromBody] BorrowDto request)
        {
            try
            {
                var borrow = await _service.GetBorrowByIdAsync(id);
                if (borrow == null)
                    return NotFound("Borrow record not found.");

                borrow.BorrowedDate = request.BorrowedDate;
                borrow.UserId = request.UserId;
                borrow.BookId = request.BookId;

                if (borrow.IsLost)
                {
                    borrow.FineAmount = 500;
                }
                else if (borrow.IsReturned && borrow.ReturnDate != null)
                {
                    var dueDate = borrow.BorrowedDate.AddDays(14);
                    var lateDays = (borrow.ReturnDate.Value - dueDate).Days;
                    borrow.FineAmount = lateDays > 0 ? lateDays * 10 : 0;
                }

                await _service.UpdateBorrowAsync(borrow);
                return Ok("Borrowed book record updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating borrow record: {ex.Message}");
            }
        }

        [HttpPut("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteBorrowedBook(int id)
        {
            try
            {
                var borrow = await _service.GetBorrowByIdAsync(id);
                if (borrow == null)
                    return NotFound("Borrowed book not found.");

                await _service.SoftDeleteBorrowAsync(borrow);
                return Ok("Borrowed book soft-deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error soft-deleting borrow record: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBorrowedBook(int id)
        {
            try
            {
                var borrow = await _service.GetBorrowByIdAsync(id);
                if (borrow == null)
                    return NotFound("Borrowed book not found.");

                await _service.DeleteBorrowAsync(borrow);
                return Ok("Borrowed book deleted permanently.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting borrow record: {ex.Message}");
            }
        }
    }
}
