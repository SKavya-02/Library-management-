using System.ComponentModel.DataAnnotations;

namespace Library.API.DTO
{
    public class ReviewDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "BookId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "BookId must be greater than 0.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Feedback is required.")]
        [MaxLength(1000, ErrorMessage = "Feedback can't exceed 1000 characters.")]
        public string Feedback { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } 
    }
}
