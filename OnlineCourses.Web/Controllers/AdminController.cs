using Microsoft.AspNetCore.Mvc;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.Web.Filters;
using OnlineCourses.Model.ViewModels;

namespace OnlineCourses.Web.Controllers
{
    [AdminMod] // Restricts entire controller to Admin/SuperAdmin only
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;

        public AdminController(IUserService userService, ICourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
        }

        public IActionResult Dashboard()
        {
            ViewBag.Message = "Welcome to Admin Dashboard";
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(CreateCourseViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _courseService.CreateCourseAsync(model);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Course created successfully!";
                return RedirectToAction("ManageCourses");
            }

            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        public async Task<IActionResult> ManageCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Course deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("ManageCourses");
        }
    }
}
