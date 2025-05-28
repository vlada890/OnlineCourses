using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourses.Model.ViewModels;

namespace OnlineCourses.BusinessLogic.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync(int? userId = null, string searchTerm = null);
        Task<CourseDetailsViewModel> GetCourseByIdAsync(int id, int? userId = null);
        Task<bool> EnrollAsync(int userId, int courseId);
        Task<UserEnrollmentsViewModel> GetUserEnrollmentsAsync(int userId);
                // =====New methods for the admin functionality using service result
        Task<ServiceResult<Course>> CreateCourseAsync(CreateCourseViewModel model);
        Task<ServiceResult<bool>> DeleteCourseAsync(int id);
        Task<ServiceResult<List<Course>>> GetCoursesByInstructorAsync(int instructorId);
        Task<ServiceResult<Course>> UpdateCourseAsync(int id, CreateCourseViewModel model);
        Task<IEnumerable<EnrollmentDetailsViewModel>> GetCourseEnrollmentsAsync(int courseId);
        Task<ServiceResult<List<CourseAdminViewModel>>> GetAllCoursesForAdminAsync();

    }
}
