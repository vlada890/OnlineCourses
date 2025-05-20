using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
                private readonly ApplicationDbContext _ctx;
        
        public CourseRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Course>> GetAllAsync() =>
            await _ctx.Courses.ToListAsync();

        public async Task<Course> GetByIdAsync(int id) =>
            await _ctx.Courses.FindAsync(id);

        public async Task<bool> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }

    }
}
