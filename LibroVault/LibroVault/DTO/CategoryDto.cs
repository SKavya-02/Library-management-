using System.ComponentModel.DataAnnotations;

namespace Library.API.DTO
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Category name can't exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}
