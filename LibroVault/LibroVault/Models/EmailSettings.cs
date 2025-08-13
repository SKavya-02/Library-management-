using System.ComponentModel.DataAnnotations;

public class EmailSettings
{
    [Required(ErrorMessage = "SMTP Server is required.")]
    public string SmtpServer { get; set; } = string.Empty;

    [Required(ErrorMessage = "Port is required.")]
    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535.")]
    public int Port { get; set; }

    [Required(ErrorMessage = "Sender name is required.")]
    [MaxLength(100, ErrorMessage = "Sender name cannot exceed 100 characters.")]
    public string SenderName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sender email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string SenderEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
