using Library.API.Interfaces;
using Library.API.Models;

namespace Library.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _repo.GetAllCategoriesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _repo.GetCategoryByIdAsync(id);
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            return await _repo.GetCategoryByNameAsync(name);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _repo.AddCategoryAsync(category);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _repo.UpdateCategoryAsync(category);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            await _repo.DeleteCategoryAsync(category);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string search)
        {
            var categories = await _repo.GetAllCategoriesAsync();

            if (!string.IsNullOrWhiteSpace(search))
                categories = categories.Where(c => c.Name.Contains(search));

            return categories;
        }
    }
}
