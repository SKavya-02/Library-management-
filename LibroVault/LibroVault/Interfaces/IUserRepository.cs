using Library.API.Models;

namespace Library.API.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<bool> SaveChangesAsync();
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> ValidateUserAsync(string email, string password);
        Task<IEnumerable<User>> SearchUsersAsync(string search, string gender, string phone);

    }
}
