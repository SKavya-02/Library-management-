using Library.API.Data;
using Library.API.Interfaces;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly LibraryDbContext _context;

        public ReviewRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Book)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.ReviewId == id);
        }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
        }

        public async Task DeleteReviewAsync(Review review)
        {
            _context.Reviews.Remove(review);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
