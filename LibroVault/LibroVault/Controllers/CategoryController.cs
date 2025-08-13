using Library.API.DTO;
using Library.API.Interfaces;
using Library.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Category name is required.");

                var existing = await _service.GetCategoryByNameAsync(request.Name);
                if (existing != null)
                    return BadRequest("Category already exists.");

                var category = new Category { Name = request.Name };
                await _service.AddCategoryAsync(category);
                return Ok("Category added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding category: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _service.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving categories: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCategory([FromQuery] string search)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(search))
                    return BadRequest("Search term is required.");

                var result = await _service.SearchCategoriesAsync(search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error searching categories: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto request)
        {
            try
            {
                var category = await _service.GetCategoryByIdAsync(id);
                if (category == null)
                    return NotFound("Category not found.");

                category.Name = request.Name;
                await _service.UpdateCategoryAsync(category);
                return Ok("Category updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating category: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _service.GetCategoryByIdAsync(id);
                if (category == null)
                    return NotFound("Category not found.");

                await _service.DeleteCategoryAsync(category);
                return Ok("Category deleted permanently.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting category: {ex.Message}");
            }
        }

        [HttpPut("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteCategory(int id)
        {
            try
            {
                var category = await _service.GetCategoryByIdAsync(id);
                if (category == null)
                    return NotFound("Category not found.");

                category.IsDeleted = true;
                await _service.UpdateCategoryAsync(category);
                return Ok("Category soft-deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error soft-deleting category: {ex.Message}");
            }
        }
    }
}
