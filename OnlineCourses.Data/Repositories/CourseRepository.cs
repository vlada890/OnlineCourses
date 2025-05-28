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
                public async Task AddAsync(Course course)
        {
            await _ctx.Courses.AddAsync(course);
        }
        public async Task DeleteAsync(int courseId)
        {
            var course = await GetByIdAsync(courseId);
            if (course != null)
            {
                _ctx.Courses.Remove(course);
            }
        }
        public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int instructorId)
        {
            return await _ctx.Courses
                .Where(c => c.InstructorId == instructorId)
                .ToListAsync();
        }
        public async Task Update(Course course)
        {
            _ctx.Courses.Update(course);
        }
    }
}
