using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Model.ViewModels;

namespace OnlineCourses.BusinessLogic.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync(int? userId = null, string searchTerm = null);
        Task<CourseDetailsViewModel> GetCourseByIdAsync(int id, int? userId = null);
        Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int instructorId);
        Task<ServiceResult<Course>> CreateCourseAsync(CourseDetailsViewModel model);
        Task<ServiceResult<Course>> UpdateCourseAsync(Course course);
        Task<ServiceResult<bool>> DeleteCourseAsync(int id);
        Task<bool> EnrollAsync(int userId, int courseId);
        Task<UserEnrollmentsViewModel> GetUserEnrollmentsAsync(int userId);
    }
}
