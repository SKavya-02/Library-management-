using Library.API.Models;

namespace Library.API.Interfaces
{
    public interface IBorrowRepository
    {
        Task<IEnumerable<BorrowedBook>> GetAllBorrowedBooksAsync();
        Task<BorrowedBook?> GetBorrowByIdAsync(int id);
        Task AddBorrowAsync(BorrowedBook borrow);
        Task UpdateBorrowAsync(BorrowedBook borrow);
        Task DeleteBorrowAsync(BorrowedBook borrow);
        Task<bool> SaveChangesAsync();
    }
}
