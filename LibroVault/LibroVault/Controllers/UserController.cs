using Library.API.DTO;
using Library.API.Interfaces;
using Library.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library.API.Controllers
{
    [Authorize]
    [ApiExplorerSettings(GroupName = "AuthUser")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IConfiguration config, IEmailService emailService)
        {
            _userService = userService;
            _config = config;
            _emailService = emailService;
        }

        // 🔐 Register User
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            try
            {
                if (request == null) return BadRequest("Invalid input.");

                var existingUser = await _userService.GetUserByEmailAsync(request.Email);
                if (existingUser != null)
                    return BadRequest("User already exists.");

                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Password = request.Password,
                    Gender = request.Gender,
                    Address = request.Address,
                    Phone = request.Phone,
                    ProfilePicture = request.ProfilePicture
                };

                await _userService.AddUserAsync(user);

                // 📧 Send Welcome Email
                var subject = "Welcome to Library World 📚✨";
                var body = $"Hi {user.FullName},<br/><br/>Thank you for registering with <b>Library World</b>!<br/>" +
                           $"You can now borrow books and enjoy reading.<br/><br/>" +
                           $"Cheers,<br/>Library World Team 💌";

                await _emailService.SendEmailAsync(user.Email, subject, body);

                return Ok("User registered successfully & welcome email sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during registration: {ex.Message}");
            }
        }

        // 🔐 Login User
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 401)]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            try
            {
                if (request == null) return BadRequest("Invalid login data.");

                var user = await _userService.ValidateUserAsync(request.Email, request.Password);
                if (user == null)
                    return Unauthorized("Invalid credentials.");

                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Login failed: {ex.Message}");
            }
        }

        // 🔑 JWT Generator
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // 📋 Get All Users
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve users: {ex.Message}");
            }
        }

        // 🔍 Search/Filter Users
        [HttpGet("search")]
        public async Task<IActionResult> SearchAndFilterUsers([FromQuery] string? search = "", [FromQuery] string? gender = "", [FromQuery] string? phone = "")
        {
            try
            {
                var users = await _userService.SearchUsersAsync(search!, gender!, phone!);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Search failed: {ex.Message}");
            }
        }

        // ✍️ Update User
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRegisterDto request)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                user.FullName = request.FullName;
                user.Email = request.Email;
                user.Phone = request.Phone;
                user.Address = request.Address;
                user.Gender = request.Gender;
                user.ProfilePicture = request.ProfilePicture;

                await _userService.UpdateUserAsync(user);
                return Ok("User updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Update failed: {ex.Message}");
            }
        }

        // ❌ Soft Delete
        [HttpPut("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                user.IsDeleted = true;
                await _userService.UpdateUserAsync(user);
                return Ok("User soft-deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Soft delete failed: {ex.Message}");
            }
        }

        // ❌ Hard Delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                await _userService.DeleteUserAsync(user);
                return Ok("User deleted permanently.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hard delete failed: {ex.Message}");
            }
        }
        [HttpPost("upload-profile-picture/{id}")]
        public async Task<IActionResult> UploadProfilePicture(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            user.ProfilePicture = $"/Uploads/{fileName}";
            await _userService.UpdateUserAsync(user);

            return Ok("Profile picture uploaded successfully.");
        }

    }
}
