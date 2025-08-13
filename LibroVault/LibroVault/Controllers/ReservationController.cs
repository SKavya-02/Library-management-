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
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationController(IReservationService service)
        {
            _service = service;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveBook([FromBody] ReservationDto request)
        {
            try
            {
                var reservation = new Reservation
                {
                    UserId = request.UserId,
                    BookId = request.BookId,
                    ReservationDate = request.ReservationDate,
                    IsActive = true
                };

                await _service.AddReservationAsync(reservation);
                return Ok("Book reserved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reserving book: {ex.Message}");
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            try
            {
                var reservation = await _service.GetReservationByIdAsync(id);
                if (reservation == null)
                    return NotFound("Reservation not found.");

                if (!reservation.IsActive)
                    return BadRequest("Reservation is already cancelled.");

                reservation.IsActive = false;
                await _service.UpdateReservationAsync(reservation);
                return Ok("Reservation cancelled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error cancelling reservation: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReservations([FromQuery] int? userId, [FromQuery] int? bookId, [FromQuery] bool? isActive)
        {
            try
            {
                var result = await _service.GetFilteredReservationsAsync(userId, bookId, isActive);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving reservations: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] ReservationDto request)
        {
            try
            {
                var reservation = await _service.GetReservationByIdAsync(id);
                if (reservation == null)
                    return NotFound("Reservation not found.");

                reservation.UserId = request.UserId;
                reservation.BookId = request.BookId;
                reservation.ReservationDate = request.ReservationDate;

                await _service.UpdateReservationAsync(reservation);
                return Ok("Reservation updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating reservation: {ex.Message}");
            }
        }

        [HttpPut("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteReservation(int id)
        {
            try
            {
                var reservation = await _service.GetReservationByIdAsync(id);
                if (reservation == null)
                    return NotFound("Reservation not found.");

                await _service.SoftDeleteReservationAsync(reservation);
                return Ok("Reservation soft-deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error soft-deleting reservation: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            try
            {
                var reservation = await _service.GetReservationByIdAsync(id);
                if (reservation == null)
                    return NotFound("Reservation not found.");

                await _service.DeleteReservationAsync(reservation);
                return Ok("Reservation deleted permanently.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting reservation: {ex.Message}");
            }
        }
    }
}
