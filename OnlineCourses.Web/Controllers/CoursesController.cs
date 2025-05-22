using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.Model.ViewModels;
using OnlineCourses.Web.Constants;
using OnlineCourses.Web.Filters;

namespace OnlineCourses.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseSvc;

        public CoursesController(ICourseService courseSvc) => _courseSvc = courseSvc;

        public async Task<IActionResult> List(string searchTerm)
        {
            var userId = HttpContext.Session.GetInt32(SessionKeys.UserId);
            var courses = await _courseSvc.GetAllCoursesAsync(userId, searchTerm);

            var viewModel = new CourseListViewModel
            {
                Courses = courses,
                IsAuthenticated = userId.HasValue,
                SearchTerm = searchTerm
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetInt32(SessionKeys.UserId);
            var course = await _courseSvc.GetCourseByIdAsync(id, userId);

            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeSession]
        public async Task<IActionResult> Enroll(EnrollmentViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Details", new { id = model.CourseId });

            var userId = HttpContext.Session.GetInt32(SessionKeys.UserId).Value;

            await _courseSvc.EnrollAsync(userId, model.CourseId);

            return RedirectToAction("Details", new { id = model.CourseId });
        }

        [HttpGet]
        [AuthorizeSession]
        public async Task<IActionResult> MyEnrollments()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeys.UserId).Value;
            var model = await _courseSvc.GetUserEnrollmentsAsync(userId);

            return View(model);
        }
    }
}
