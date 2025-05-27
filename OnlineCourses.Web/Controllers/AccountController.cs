using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.Model.ViewModels;
using OnlineCourses.Web.Constants;
namespace OnlineCourses.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userSvc;

        public AccountController(IUserService userSvc) => _userSvc = userSvc;

        [HttpGet]
        public IActionResult Register()
        {
            // Redirect if already logged in
            if (HttpContext.Session.GetInt32(SessionKeys.UserId).HasValue)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _userSvc.RegisterAsync(vm);

            if (result.Success)
                return RedirectToAction("Login");

            ModelState.AddModelError("", result.Message);
            return View(vm);
        }


        [HttpGet]
        public IActionResult Login()
        {
            // Redirect if already logged in
            if (HttpContext.Session.GetInt32(SessionKeys.UserId).HasValue)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _userSvc.ValidateUserAsync(vm);

            if (result.Success)
            {
                // Store session data
                HttpContext.Session.SetInt32(SessionKeys.UserId, result.User.Id);
                HttpContext.Session.SetString(SessionKeys.UserName, result.User.FullName);
                HttpContext.Session.SetString(SessionKeys.UserEmail, result.User.Email);
                HttpContext.Session.SetString(SessionKeys.UserRole, result.User.Role.ToString());

                // Redirect based on role
                return result.User.Role switch
                {
                    Domain.Entities.UserRole.Admin =>
                        RedirectToAction("Dashboard", "Admin"),
                    Domain.Entities.UserRole.Instructor =>
                        RedirectToAction("Dashboard", "Instructor"),
                    _ => RedirectToAction("List", "Courses")
                };
            }

            ModelState.AddModelError("", "Invalid credentials");
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            ViewBag.Message = "You don't have permission to access this resource.";
            return View();
        }
    }
}
