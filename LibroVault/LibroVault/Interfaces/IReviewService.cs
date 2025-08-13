using Library.API.Models;

namespace Library.API.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> FilterReviewsAsync(int? bookId, int? rating);
        Task<Review?> GetReviewByIdAsync(int id);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task SoftDeleteReviewAsync(Review review);
        Task DeleteReviewAsync(Review review);
    }
}
