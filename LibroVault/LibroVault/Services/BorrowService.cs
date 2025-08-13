using Library.API.Interfaces;
using Library.API.Models;

namespace Library.API.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _repo;

        public BorrowService(IBorrowRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<BorrowedBook>> GetAllBorrowedBooksAsync(int? userId, int? bookId, bool? isReturned)
        {
            var all = await _repo.GetAllBorrowedBooksAsync();

            if (userId.HasValue)
                all = all.Where(b => b.UserId == userId.Value);

            if (bookId.HasValue)
                all = all.Where(b => b.BookId == bookId.Value);

            if (isReturned.HasValue)
                all = all.Where(b => b.IsReturned == isReturned.Value);

            return all;
        }

        public async Task<BorrowedBook?> GetBorrowByIdAsync(int id)
        {
            return await _repo.GetBorrowByIdAsync(id);
        }

        public async Task AddBorrowAsync(BorrowedBook borrow)
        {
            await _repo.AddBorrowAsync(borrow);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateBorrowAsync(BorrowedBook borrow)
        {
            await _repo.UpdateBorrowAsync(borrow);
            await _repo.SaveChangesAsync();
        }

        public async Task SoftDeleteBorrowAsync(BorrowedBook borrow)
        {
            borrow.IsDeleted = true;
            await _repo.UpdateBorrowAsync(borrow);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteBorrowAsync(BorrowedBook borrow)
        {
            await _repo.DeleteBorrowAsync(borrow);
            await _repo.SaveChangesAsync();
        }
    }
}
