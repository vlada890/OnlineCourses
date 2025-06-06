using Microsoft.AspNetCore.Mvc;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.Web.Filters;
using OnlineCourses.Web.Constants;

namespace OnlineCourses.Web.Controllers
{
    [InstructorMod] // Restricts to Instructor, Admin, SuperAdmin
    public class InstructorController : Controller
    {
        private readonly ICourseService _courseService;

        public InstructorController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Dashboard()
        {
            ViewBag.Message = "Welcome to Instructor Dashboard";
            return View();
        }

        public async Task<IActionResult> MyCourses()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeys.UserId).Value;
            var courses = await _courseService.GetCoursesByInstructorAsync(userId);
            return View(courses);
        }

        [RequireRole(Domain.Entities.UserRole.Instructor)] // Only instructors can create courses
        public IActionResult CreateCourse()
        {
            return View();
        }

        // Specific action restricted to Admin only within Instructor controller
        [AdminMod]
        public async Task<IActionResult> ManageAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }
    }
}
