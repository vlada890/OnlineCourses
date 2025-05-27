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

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _ctx.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _ctx.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task AddAsync(Course course)
        {
            await _ctx.Courses.AddAsync(course);
        }
        public async Task<IEnumerable<Course>> GetByInstructorAsync(int instructorId)
        {
            return await _ctx.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Where(c => c.InstructorId == instructorId)
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<Course> GetCourseByTitleAsync(string title) 
        {
            return await _ctx.Courses
                             .SingleOrDefaultAsync(c => c.Title.ToLower() == title.ToLower());
        }
        public async Task<Course> CreateAsync(Course course)
        {
            _ctx.Courses.Add(course);
            await _ctx.SaveChangesAsync();
            return await GetByIdAsync(course.Id);
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            _ctx.Courses.Update(course);
            await _ctx.SaveChangesAsync();
            return await GetByIdAsync(course.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _ctx.Courses.FindAsync(id);
            if (course == null) return false;

            _ctx.Courses.Remove(course);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _ctx.Courses.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }

    }
}
