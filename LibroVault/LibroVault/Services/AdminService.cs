using Library.API.Interfaces;
using Library.API.Models;

namespace Library.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repo;

        public AdminService(IAdminRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _repo.GetAllAdminsAsync();
        }

        public async Task<Admin?> GetAdminByIdAsync(int id)
        {
            return await _repo.GetAdminByIdAsync(id);
        }

        public async Task<Admin?> GetAdminByEmailAsync(string email)
        {
            return await _repo.GetAdminByEmailAsync(email);
        }

        public async Task AddAdminAsync(Admin admin)
        {
            await _repo.AddAdminAsync(admin);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAdminAsync(Admin admin)
        {
            await _repo.UpdateAdminAsync(admin);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAdminAsync(Admin admin)
        {
            await _repo.DeleteAdminAsync(admin);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<Admin>> SearchAdminsAsync(string search, string emailDomain)
        {
            var admins = await _repo.GetAllAdminsAsync();

            if (!string.IsNullOrWhiteSpace(search))
                admins = admins.Where(a => a.FullName.Contains(search) || a.Email.Contains(search));

            if (!string.IsNullOrWhiteSpace(emailDomain))
                admins = admins.Where(a => a.Email.EndsWith(emailDomain));

            return admins;
        }
    }
}
