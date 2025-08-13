using System.ComponentModel.DataAnnotations;

namespace Library.API.DTO
{
    public class BookDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title can't exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required.")]
        [MaxLength(150, ErrorMessage = "Author name can't exceed 150 characters.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN is required.")]
        [MaxLength(20, ErrorMessage = "ISBN can't exceed 20 characters.")]
        public string ISBN { get; set; } = string.Empty;

        [MaxLength(150, ErrorMessage = "Publisher can't exceed 150 characters.")]
        public string Publisher { get; set; } = string.Empty;

        [Required(ErrorMessage = "Publication date is required.")]
        public DateTime PublicationDate { get; set; }

        [MaxLength(50, ErrorMessage = "Edition can't exceed 50 characters.")]
        public string Edition { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Language can't exceed 50 characters.")]
        public string Language { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Number of pages must be at least 1.")]
        public int NumberOfPages { get; set; }

        [MaxLength(1000, ErrorMessage = "Description can't exceed 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 999999.99, ErrorMessage = "Cost must be a valid number.")]
        public decimal Cost { get; set; }

        [MaxLength(500, ErrorMessage = "Image URL can't exceed 500 characters.")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; set; }
    }
}
