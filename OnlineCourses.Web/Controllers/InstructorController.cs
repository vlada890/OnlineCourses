using Microsoft.AspNetCore.Mvc;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.Web.Filters;
using OnlineCourses.Web.Constants;
using OnlineCourses.Model.ViewModels;

namespace OnlineCourses.Web.Controllers
{
    [InstructorMod] // Restricts to Instructor, Admin, SuperAdmin
    public class InstructorController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<InstructorController> _logger;

        public InstructorController(ICourseService courseService, ILogger<InstructorController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            ViewBag.Message = "Welcome to Instructor Dashboard";
            return View();
        }

        public async Task<IActionResult> MyCourses()
        {
            try
            {

                var userId = HttpContext.Session.GetInt32(SessionKeys.UserId);
                if (!userId.HasValue)
                {
                    _logger.LogWarning("User ID not found in session");
                    return RedirectToAction("Login", "Account");
                }

                var courses = await _courseService.GetCoursesByInstructorAsync(userId.Value);
                return View(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving courses for instructor");
                TempData["ErrorMessage"] = "An error occurred while loading your courses.";
                return View();
            }

        }

        [RequireRole(Domain.Entities.UserRole.Instructor)] // Only instructors can create courses
        public IActionResult CreateCourse()
        {
            return View(new CreateCourseViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(CreateCourseViewModel viewModel)
        {
            try
            {
                // DEBUG: Log all received values
                _logger.LogInformation("DEBUG: Received form data - Title: {Title}, Description: {Description}, Duration: {Duration}, InstructorId: {InstructorId}",
                    viewModel?.Title, viewModel?.Description, viewModel?.Duration, viewModel?.InstructorId);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for course creation");

                    // DEBUG: Show all validation errors
                    foreach (var modelError in ModelState)
                    {
                        var key = modelError.Key;
                        var errors = modelError.Value.Errors;
                        foreach (var error in errors)
                        {
                            _logger.LogError("Validation Error - Key: {Key}, Error: {Error}", key, error.ErrorMessage);
                        }
                    }

                    return View(viewModel);
                }

                var userIdNullable = HttpContext.Session.GetInt32(SessionKeys.UserId);
                if (!userIdNullable.HasValue)
                {
                    _logger.LogWarning("User Id not found in session during course creation");
                    return RedirectToAction("Login", "Account");
                }

                viewModel.InstructorId = userIdNullable.Value;
                _logger.LogInformation("Creating Course for {InstructorId}: {Title}", userIdNullable, viewModel.Title);

                var result = await _courseService.CreateCourseAsync(viewModel);

                if (result.Success)
                {
                    _logger.LogInformation("Course created successfully: {Title}", viewModel.Title);
                    return RedirectToAction("MyCourses");
                }
                else
                {
                    _logger.LogWarning("Failed to create course: {Message}", result.Message);
                    ModelState.AddModelError("", result.Message);
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course: {Title}", viewModel?.Title);
                ModelState.AddModelError("", "An unexpected error occurred while creating the course.");
                return View(viewModel);
            }
        }
        // Add these methods to your InstructorController class
        [HttpGet]
        [RequireRole(Domain.Entities.UserRole.Instructor)]
        public async Task<IActionResult> EditCourse(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32(SessionKeys.UserId);
                if (!userId.HasValue)
                {
                    _logger.LogWarning("User ID not found in session");
                    return RedirectToAction("Login", "Account");
                }

                // Get the course details
                var courseDetails = await _courseService.GetCourseByIdAsync(id, userId.Value);
                if (courseDetails == null)
                {
                    _logger.LogWarning("Course not found with ID: {CourseId}", id);
                    TempData["ErrorMessage"] = "Course not found.";
                    return RedirectToAction("MyCourses");
                }
                // Map to EditCourseViewModel
                var editViewModel = new EditCourseViewModel
                {
                    Id = courseDetails.Id,
                    Title = courseDetails.Title,
                    Description = courseDetails.Description,
                    Duration = courseDetails.Duration

                };

                return View(editViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving course for editing: {CourseId}", id);
                TempData["ErrorMessage"] = "An error occurred while loading the course.";
                return RedirectToAction("MyCourses");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireRole(Domain.Entities.UserRole.Instructor)]
        public async Task<IActionResult> EditCourse(EditCourseViewModel viewModel)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32(SessionKeys.UserId);
                if (!userId.HasValue)
                {
                    _logger.LogWarning("User ID not found in session during course update");
                    return RedirectToAction("Login", "Account");
                }

                // Set the InstructorId before validation
                viewModel.InstructorId = userId.Value;

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for course update");

                    // Log validation errors for debugging
                    foreach (var modelError in ModelState)
                    {
                        var key = modelError.Key;
                        var errors = modelError.Value.Errors;
                        foreach (var error in errors)
                        {
                            _logger.LogError("Validation Error - Key: {Key}, Error: {Error}", key, error.ErrorMessage);
                        }
                    }

                    return View(viewModel);
                }

                // Create a CreateCourseViewModel for the service (since UpdateCourseAsync expects it)
                var updateModel = new CreateCourseViewModel
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Duration = viewModel.Duration,
                    InstructorId = viewModel.InstructorId
                };

                _logger.LogInformation("Updating Course {CourseId} for Instructor {InstructorId}: {Title}",
                    viewModel.Id, userId.Value, viewModel.Title);

                var result = await _courseService.UpdateCourseAsync(viewModel.Id, updateModel);

                if (result.Success)
                {
                    _logger.LogInformation("Course updated successfully: {Title}", viewModel.Title);
                    TempData["SuccessMessage"] = "Course updated successfully!";
                    return RedirectToAction("MyCourses");
                }
                else
                {
                    _logger.LogWarning("Failed to update course: {Message}", result.Message);
                    ModelState.AddModelError("", result.Message);
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course: {CourseId}", viewModel?.Id);
                ModelState.AddModelError("", "An unexpected error occurred while updating the course.");
                return View(viewModel);
            }
        }
        // Specific action restricted to Admin only within Instructor controller
        [AdminMod]
        public async Task<IActionResult> ManageAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        // Add this method to your InstructorController class

        [RequireRole(Domain.Entities.UserRole.Instructor)]
        public async Task<IActionResult> CourseStudents(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32(SessionKeys.UserId);
                if (!userId.HasValue)
                {
                    _logger.LogWarning("User ID not found in session");
                    return RedirectToAction("Login", "Account");
                }

                // First, verify that the course exists and belongs to the current instructor
                var courseDetails = await _courseService.GetCourseByIdAsync(id, userId.Value);
                if (courseDetails == null)
                {
                    _logger.LogWarning("Course not found with ID: {CourseId}", id);
                    TempData["ErrorMessage"] = "Course not found.";
                    return RedirectToAction("MyCourses");
                }

                // Check if the current user is the instructor of this course
                if (courseDetails.InstructorId != userId.Value)
                {
                    _logger.LogWarning("User {UserId} attempted to view students for course {CourseId} they don't own", userId.Value, id);
                    TempData["ErrorMessage"] = "You can only view students for your own courses.";
                    return RedirectToAction("MyCourses");
                }

                // Get the enrolled students for this course
                var enrollments = await _courseService.GetCourseEnrollmentsAsync(id);

                // Create a view model that includes course info and enrollments
                var viewModel = new CourseStudentsViewModel
                {
                    CourseId = courseDetails.Id,
                    CourseTitle = courseDetails.Title,
                    CourseDescription = courseDetails.Description,
                    InstructorName = courseDetails.InstructorName,
                    Enrollments = enrollments.ToList()
                };

                _logger.LogInformation("Retrieved {Count} students for course {CourseId}", enrollments.Count(), id);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving students for course: {CourseId}", id);
                TempData["ErrorMessage"] = "An error occurred while loading the course students.";
                return RedirectToAction("MyCourses");
            }
        }
    }

}
