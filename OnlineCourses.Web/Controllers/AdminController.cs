using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.Web.Filters;
using OnlineCourses.Model.ViewModels;
using OnlineCourses.Data;
using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Web.Controllers
{
    [AdminMod] // Restricts entire controller to Admin/SuperAdmin only
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly ApplicationDbContext _context;

        public AdminController(IUserService userService, ICourseService courseService, ApplicationDbContext context)
        {
            _userService = userService;
            _courseService = courseService;
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewBag.Message = "Welcome to Admin Dashboard";
            return View();
        }
        public async Task<IActionResult> Courses()
        {
            var result = await _courseService.GetAllCoursesForAdminAsync();

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(new List<CourseAdminViewModel>());
            }

            return View(result.Data); // This must be List<CourseAdminViewModel>
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users.Data);
        }
/*
        [HttpGet]
        public async Task<IActionResult> CreateCourse()
        {
            await PopulateInstructorsDropdown();
            return View(new CreateCourseViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(CreateCourseViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateInstructorsDropdown();
                    return View(model);
                }

                var result = await _courseService.CreateCourseAsync(model);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Course created successfully!";
                    return RedirectToAction("Courses");
                }

                ModelState.AddModelError("", result.Message);
                await PopulateInstructorsDropdown();
                return View(model);
            }
            catch (Exception ex)
            {
            //    _logger.LogError(ex, "Error creating course: {Title}", model?.Title);
                ModelState.AddModelError("", "An unexpected error occurred while creating the course.");
                await PopulateInstructorsDropdown();
                return View(model);
            }
        }

        private async Task PopulateInstructorsDropdown()
        {
            var instructors = await _context.Users
                .Where(u => u.Role == UserRole.Instructor || u.Role == UserRole.Admin || u.Role == UserRole.SuperAdmin)
                .OrderBy(u => u.FullName)
                .ToListAsync();

            ViewBag.Instructors = instructors;
        }
*/

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
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id); 
            return RedirectToAction("Users");
        }
                [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(EditUserViewModel updatedUser)
        {
            if (!ModelState.IsValid)
                return View(updatedUser);
    
            var user = _context.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user == null)
                return NotFound();
    
                user.Role = updatedUser.Role;
                user.Email = updatedUser.Email;
                user.FullName = updatedUser.FullName;
    
            _context.SaveChanges();
    
            return RedirectToAction("Users");
        }

    }
}
