using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Data.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetByInstructorAsync(int instructorId);
        Task AddAsync(Course course);
        Task<Course> CreateAsync(Course course);
        Task<Course> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<Course> GetCourseByTitleAsync(string title);
        Task<bool> SaveChangesAsync();

    }
}
