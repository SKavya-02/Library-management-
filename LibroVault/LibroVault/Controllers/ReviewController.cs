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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddReview([FromBody] ReviewDto request)
        {
            try
            {
                var review = new Review
                {
                    UserId = request.UserId,
                    BookId = request.BookId,
                    Feedback = request.Feedback,
                    Rating = request.Rating,
                    CreatedAt = DateTime.Now
                };

                await _service.AddReviewAsync(review);
                return Ok("Review added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding review: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = await _service.FilterReviewsAsync(null, null);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving reviews: {ex.Message}");
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterReviews([FromQuery] int? bookId, [FromQuery] int? rating)
        {
            try
            {
                var reviews = await _service.FilterReviewsAsync(bookId, rating);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error filtering reviews: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDto request)
        {
            try
            {
                var review = await _service.GetReviewByIdAsync(id);
                if (review == null)
                    return NotFound("Review not found.");

                review.Feedback = request.Feedback;
                review.Rating = request.Rating;
                review.CreatedAt = DateTime.Now;

                await _service.UpdateReviewAsync(review);
                return Ok("Review updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating review: {ex.Message}");
            }
        }

        [HttpPut("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteReview(int id)
        {
            try
            {
                var review = await _service.GetReviewByIdAsync(id);
                if (review == null)
                    return NotFound("Review not found.");

                await _service.SoftDeleteReviewAsync(review);
                return Ok("Review soft-deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error soft-deleting review: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                var review = await _service.GetReviewByIdAsync(id);
                if (review == null)
                    return NotFound("Review not found.");

                await _service.DeleteReviewAsync(review);
                return Ok("Review deleted permanently.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting review: {ex.Message}");
            }
        }
    }
}
