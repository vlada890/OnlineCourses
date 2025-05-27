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
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> SaveChangesAsync();
    }
}
