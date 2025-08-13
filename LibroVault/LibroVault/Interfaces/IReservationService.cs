using Library.API.Models;

namespace Library.API.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetFilteredReservationsAsync(int? userId, int? bookId, bool? isActive);
        Task<Reservation?> GetReservationByIdAsync(int id);
        Task AddReservationAsync(Reservation reservation);
        Task UpdateReservationAsync(Reservation reservation);
        Task SoftDeleteReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(Reservation reservation);
    }
}
