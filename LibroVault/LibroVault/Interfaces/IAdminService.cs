using Library.API.Models;

namespace Library.API.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<Admin>> GetAllAdminsAsync();
        Task<Admin?> GetAdminByIdAsync(int id);
        Task<Admin?> GetAdminByEmailAsync(string email);
        Task AddAdminAsync(Admin admin);
        Task UpdateAdminAsync(Admin admin);
        Task DeleteAdminAsync(Admin admin);
        Task<IEnumerable<Admin>> SearchAdminsAsync(string search, string emailDomain);
    }
}
