using System.ComponentModel.DataAnnotations;

namespace Library.API.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Password { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

    }
}
