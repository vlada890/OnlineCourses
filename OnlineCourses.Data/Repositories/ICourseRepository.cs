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
        Task<bool> SaveChangesAsync();
        Task AddAsync(Course course);
        Task DeleteAsync(int courseId);
        Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int instructor);
        Task Update(Course course);

    }
}
