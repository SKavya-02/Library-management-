using Library.API.Interfaces;
using Library.API.Models;

namespace Library.API.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repo;

        public ReservationService(IReservationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Reservation>> GetFilteredReservationsAsync(int? userId, int? bookId, bool? isActive)
        {
            var all = await _repo.GetAllReservationsAsync();

            if (userId.HasValue)
                all = all.Where(r => r.UserId == userId.Value);

            if (bookId.HasValue)
                all = all.Where(r => r.BookId == bookId.Value);

            if (isActive.HasValue)
                all = all.Where(r => r.IsActive == isActive.Value);

            return all;
        }

        public async Task<Reservation?> GetReservationByIdAsync(int id)
        {
            return await _repo.GetReservationByIdAsync(id);
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            await _repo.AddReservationAsync(reservation);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            await _repo.UpdateReservationAsync(reservation);
            await _repo.SaveChangesAsync();
        }

        public async Task SoftDeleteReservationAsync(Reservation reservation)
        {
            reservation.IsActive = false;
            await _repo.UpdateReservationAsync(reservation);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteReservationAsync(Reservation reservation)
        {
            await _repo.DeleteReservationAsync(reservation);
            await _repo.SaveChangesAsync();
        }
    }
}
