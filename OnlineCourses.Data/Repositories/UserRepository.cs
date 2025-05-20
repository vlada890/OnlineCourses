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

        public async Task AddAsync(User user)
        {
            await _ctx.Users.AddAsync(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}
