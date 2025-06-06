using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int userId);
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task DeleteAsync(int userId);
        Task UpdateAsync(User user);

        Task<bool> EmailExistsAsync(string email);
    }
}
