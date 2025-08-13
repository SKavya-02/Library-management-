using Library.API.Data;
using Library.API.Interfaces;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibraryDbContext _context;

        public BorrowRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BorrowedBook>> GetAllBorrowedBooksAsync()
        {
            return await _context.BorrowedBooks
                .Include(b => b.User)
                .Include(b => b.Book)
                .Where(b => !b.IsDeleted)
                .ToListAsync();
        }

        public async Task<BorrowedBook?> GetBorrowByIdAsync(int id)
        {
            return await _context.BorrowedBooks
                .Include(b => b.User)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.BorrowedBookId == id);
        }

        public async Task AddBorrowAsync(BorrowedBook borrow)
        {
            await _context.BorrowedBooks.AddAsync(borrow);
        }

        public async Task UpdateBorrowAsync(BorrowedBook borrow)
        {
            _context.BorrowedBooks.Update(borrow);
        }

        public async Task DeleteBorrowAsync(BorrowedBook borrow)
        {
            _context.BorrowedBooks.Remove(borrow);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
