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
    }
}
