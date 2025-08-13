using Library.API.Interfaces;
using Library.API.Models;

namespace Library.API.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repo;

        public ReviewService(IReviewRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Review>> FilterReviewsAsync(int? bookId, int? rating)
        {
            var allReviews = await _repo.GetAllReviewsAsync();

            if (bookId.HasValue)
                allReviews = allReviews.Where(r => r.BookId == bookId.Value);

            if (rating.HasValue)
                allReviews = allReviews.Where(r => r.Rating == rating.Value);

            return allReviews;
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _repo.GetReviewByIdAsync(id);
        }

        public async Task AddReviewAsync(Review review)
        {
            await _repo.AddReviewAsync(review);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(Review review)
        {
            await _repo.UpdateReviewAsync(review);
            await _repo.SaveChangesAsync();
        }

        public async Task SoftDeleteReviewAsync(Review review)
        {
            review.IsDeleted = true;
            await _repo.UpdateReviewAsync(review);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(Review review)
        {
            await _repo.DeleteReviewAsync(review);
            await _repo.SaveChangesAsync();
        }
    }
}
