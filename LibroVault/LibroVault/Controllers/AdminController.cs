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
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IConfiguration _config;

        public AdminController(IAdminService adminService, IConfiguration config)
        {
            _adminService = adminService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin(AdminDto request)
        {
            try
            {
                if (await _adminService.GetAdminByEmailAsync(request.Email) != null)
                    return BadRequest("Admin already exists.");

                var admin = new Admin
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Password = request.Password
                };

                await _adminService.AddAdminAsync(admin);
                return Ok("Admin registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error during registration: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAdmin(AdminDto request)
        {
            try
            {
                var admin = await _adminService.GetAdminByEmailAsync(request.Email);
                if (admin == null || admin.Password != request.Password)
                    return Unauthorized("Invalid credentials.");

                var token = GenerateJwtToken(admin);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error during login: {ex.Message}");
            }
        }

        private string GenerateJwtToken(Admin admin)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, admin.FullName),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Role, "Admin")
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

        [HttpGet("search")]
        public async Task<IActionResult> SearchAdmins([FromQuery] string? search = "", [FromQuery] string? emailDomain = "")
        {
            try
            {
                var result = await _adminService.SearchAdminsAsync(search!, emailDomain!);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to search admins: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, [FromBody] AdminDto request)
        {
            try
            {
                var admin = await _adminService.GetAdminByIdAsync(id);
                if (admin == null)
                    return NotFound("Admin not found.");

                admin.FullName = request.FullName;
                admin.Email = request.Email;

                await _adminService.UpdateAdminAsync(admin);
                return Ok("Admin updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update admin: {ex.Message}");
            }
        }

        [HttpPut("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteAdmin(int id)
        {
            try
            {
                var admin = await _adminService.GetAdminByIdAsync(id);
                if (admin == null)
                    return NotFound("Admin not found.");

                admin.IsDeleted = true;
                await _adminService.UpdateAdminAsync(admin);
                return Ok("Admin soft-deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to soft delete admin: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            try
            {
                var admin = await _adminService.GetAdminByIdAsync(id);
                if (admin == null)
                    return NotFound("Admin not found.");

                await _adminService.DeleteAdminAsync(admin);
                return Ok("Admin deleted permanently.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to delete admin: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var admins = await _adminService.GetAllAdminsAsync();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to fetch admins: {ex.Message}");
            }
        }
    }
}
