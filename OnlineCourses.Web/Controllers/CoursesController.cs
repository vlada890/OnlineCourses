/*using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        [AdminMod] // Only Admin can access
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminMod] // Only Admin can create courses
        public async Task<IActionResult> Create(CourseDetailsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _courseSvc.CreateCourseAsync(model);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Course created successfully!";
                return RedirectToAction("List");
            }

            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminMod] // Only Admin/SuperAdmin can delete courses
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseSvc.DeleteCourseAsync(id);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Course deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("List");
        }
    }
}*/
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
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseService courseSvc, ILogger<CoursesController> logger)
        {
            _courseSvc = courseSvc;
            _logger = logger;
        }

        public async Task<IActionResult> List(string searchTerm)
        {
            try
            {
                _logger.LogInformation("Starting List action with searchTerm: {SearchTerm}", searchTerm);

                var userId = HttpContext.Session.GetInt32(SessionKeys.UserId);
                _logger.LogInformation("User ID from session: {UserId}", userId);

                var courses = await _courseSvc.GetAllCoursesAsync(userId, searchTerm);
                _logger.LogInformation("Retrieved {CourseCount} courses from service", courses?.Count() ?? 0);

                if (courses != null)
                {
                    foreach (var course in courses)
                    {
                        _logger.LogInformation("Course: ID={Id}, Title={Title}, Instructor={Instructor}",
                            course.Id, course.Title, course.Instructor ?? "NULL");
                    }
                }

                var viewModel = new CourseListViewModel
                {
                    Courses = courses ?? new List<CourseDto>(),
                    IsAuthenticated = userId.HasValue,
                    SearchTerm = searchTerm
                };

                _logger.LogInformation("Returning view with {CourseCount} courses, IsAuthenticated: {IsAuth}",
                    viewModel.Courses.Count(), viewModel.IsAuthenticated);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in List action");
                throw;
            }
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

        [HttpGet]
        [AdminMod] // Only Admin can access
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminMod] // Only Admin/SuperAdmin can create courses
        public async Task<IActionResult> Create(CourseDetailsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _courseSvc.CreateCourseAsync(model);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Course created successfully!";
                return RedirectToAction("List");
            }
            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminMod] // Only Admin/SuperAdmin can delete courses
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseSvc.DeleteCourseAsync(id);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Course deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            return RedirectToAction("List");
        }
    }
}
