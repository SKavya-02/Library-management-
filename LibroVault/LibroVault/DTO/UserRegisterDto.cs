using System.ComponentModel.DataAnnotations;

namespace Library.API.DTO
{
    public class UserRegisterDto
    {
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ProfilePicture { get; set; }
    }
}
