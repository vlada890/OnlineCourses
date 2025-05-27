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

        public async Task<User> GetByIdAsync(int id)
        {
            return await _ctx.Users
                .Include(u => u.Enrollments)
                .ThenInclude(e => e.Course)
                .Include(u => u.CoursesCreated)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email) =>
            await _ctx.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());


        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _ctx.Users
                .Include(u => u.Enrollments)
                .Include(u => u.CoursesCreated)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
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

        public async Task<User> CreateAsync(User user)
        {
            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _ctx.Users.FindAsync(id);
            if (user == null) return false;

            _ctx.Users.Remove(user);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _ctx.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}
