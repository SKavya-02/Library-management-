using Library.API.Interfaces;
using Library.API.Models;

namespace Library.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repo.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _repo.GetUserByIdAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await _repo.AddUserAsync(user);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _repo.UpdateUserAsync(user);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            await _repo.DeleteUserAsync(user);
            await _repo.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _repo.GetUserByEmailAsync(email);
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            return await _repo.ValidateUserAsync(email, password);
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string search, string gender, string phone)
        {
            return await _repo.SearchUsersAsync(search, gender, phone);
        }
    }
}
