using Library.API.Models;

namespace Library.API.Interfaces
{
    public interface IBorrowService
    {
        Task<IEnumerable<BorrowedBook>> GetAllBorrowedBooksAsync(int? userId, int? bookId, bool? isReturned);
        Task<BorrowedBook?> GetBorrowByIdAsync(int id);
        Task AddBorrowAsync(BorrowedBook borrow);
        Task UpdateBorrowAsync(BorrowedBook borrow);
        Task SoftDeleteBorrowAsync(BorrowedBook borrow);
        Task DeleteBorrowAsync(BorrowedBook borrow);
    }
}
