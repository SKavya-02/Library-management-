using System.ComponentModel.DataAnnotations;

namespace Library.API.DTO
{
    public class BorrowDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "BookId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "BookId must be greater than 0.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "BorrowedDate is required.")]
        public DateTime BorrowedDate { get; set; }
    }
}
