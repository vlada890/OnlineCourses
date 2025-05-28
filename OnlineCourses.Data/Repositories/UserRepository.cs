using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Entities;


namespace OnlineCourses.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _ctx;

        public UserRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<User> GetByIdAsync(int userId) =>
            await _ctx.Users.FindAsync(userId);

        public async Task<User> GetByEmailAsync(string email) =>
            await _ctx.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _ctx.Users
                .Include(u => u.Enrollments)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _ctx.Users
                .Where(u => u.Role == role)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _ctx.Users.AddAsync(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
         public async Task DeleteAsync(int userId)
         {
             var user = await _ctx.Users.FindAsync(userId);
             if (user != null)
             {
                 _ctx.Users.Remove(user);
             }
         }
         public async Task UpdateAsync(User user)
         {
             _ctx.Users.Update(user);
         }
        
         public async Task<bool> EmailExistsAsync(string email)
         {
             return await _ctx.Users
                 .AnyAsync(u => u.Email.ToLower() == email.ToLower());
         }
    }
}
