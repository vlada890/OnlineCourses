using AutoMapper;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.Data.Repositories;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Model.ViewModels;

namespace OnlineCourses.BusinessLogic.Services
{
    public class CourseService : ICourseService
    {

        private readonly ICourseRepository _courseRepo;
        private readonly IUserRepository _userRepo;
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IMapper _mapper;

        public CourseService(
            ICourseRepository courseRepo,
            IUserRepository userRepo,
            IEnrollmentRepository enrollmentRepo,
            IMapper mapper)
        {
            _courseRepo = courseRepo;
            _userRepo = userRepo;
            _enrollmentRepo = enrollmentRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync(int? userId = null, string searchTerm = null)
        {
            try
            {
                Console.WriteLine($"CourseService: Getting all courses. UserId: {userId}, SearchTerm: {searchTerm}");

                var courses = await _courseRepo.GetAllAsync();
                Console.WriteLine($"CourseService: Retrieved {courses?.Count() ?? 0} courses from repository");

                if (courses != null)
                {
                    foreach (var course in courses)
                    {
                        Console.WriteLine($"Course from DB: ID={course.Id}, Title={course.Title}, InstructorId={course.InstructorId}, Instructor={course.Instructor?.FullName ?? "NULL"}");
                    }
                }

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.Trim().ToLower();
                    courses = courses.Where(c =>
                        (c.Title?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (c.Description?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (c.Instructor?.FullName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false));
                    Console.WriteLine($"CourseService: After search filter: {courses?.Count() ?? 0} courses");
                }

                var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);
                Console.WriteLine($"CourseService: After mapping: {courseDtos?.Count() ?? 0} DTOs");

                if (courseDtos != null)
                {
                    foreach (var dto in courseDtos)
                    {
                        Console.WriteLine($"CourseDTO: ID={dto.Id}, Title={dto.Title}, Instructor={dto.Instructor ?? "NULL"}");
                    }
                }

                if (userId.HasValue)
                {
                    foreach (var course in courseDtos)
                    {
                        course.IsEnrolled = await _enrollmentRepo.IsUserEnrolledAsync(userId.Value, course.Id);
                    }
                }

                return courseDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CourseService Error: {ex.Message}");
                Console.WriteLine($"CourseService Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<CourseDetailsViewModel> GetCourseByIdAsync(int id, int? userId = null)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            if (course == null)
                return null;

            var viewModel = _mapper.Map<CourseDetailsViewModel>(course);

            if (userId.HasValue)
            {
                viewModel.IsAuthenticated = true;
                viewModel.IsEnrolled = await _enrollmentRepo.IsUserEnrolledAsync(userId.Value, id);
            }

            return viewModel;
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int instructorId)
        {
            return await _courseRepo.GetByInstructorAsync(instructorId);
        }
        public async Task<ServiceResult<Course>> CreateCourseAsync(CourseDetailsViewModel model)
        {
            try
            {
                // Verify instructor exists
                var instructor = await _userRepo.GetByIdAsync(model.InstructorId);
                if (instructor == null)
                {
                    return ServiceResult<Course>.FailureResult("Instructor not found");
                }

                // Verify instructor has appropriate role
                if (instructor.Role != UserRole.Instructor &&
                    instructor.Role != UserRole.Admin)
                {
                    return ServiceResult<Course>.FailureResult("User does not have permission to create courses");
                }

                var course = new Course
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    ImageUrl = model.ImageUrl,
                    InstructorId = model.InstructorId,
                    CreatedAt = DateTime.UtcNow
                };

                var createdCourse = await _courseRepo.CreateAsync(course);
                return ServiceResult<Course>.SuccessResult(createdCourse, "Course created successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<Course>.FailureResult($"Course creation failed: {ex.Message}");
            }
        }
        public async Task<ServiceResult<Course>> UpdateCourseAsync(Course course)
        {
            try
            {
                var updatedCourse = await _courseRepo.UpdateAsync(course);
                return ServiceResult<Course>.SuccessResult(updatedCourse, "Course updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<Course>.FailureResult($"Course update failed: {ex.Message}");
            }
        }
        public async Task<ServiceResult<bool>> DeleteCourseAsync(int id)
        {
            try
            {
                var result = await _courseRepo.DeleteAsync(id);
                return result ?
                    ServiceResult<bool>.SuccessResult(true, "Course deleted successfully") :
                    ServiceResult<bool>.FailureResult("Course not found");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.FailureResult($"Course deletion failed: {ex.Message}");
            }
        }
        public async Task<bool> EnrollAsync(int userId, int courseId)
        {
            //check if course exists
            if (!await _courseRepo.ExistsAsync(courseId))
                return false;
            //check if user exists
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return false;
            // Check if already enrolled
            if (await _enrollmentRepo.IsUserEnrolledAsync(userId, courseId))
                return false;

            // Create enrollment
            var enrollment = new Enrollment
            {
                UserId = userId,
                CourseId = courseId,
                EnrolledOn = DateTime.UtcNow
            };

            await _enrollmentRepo.AddAsync(enrollment);
            return await _enrollmentRepo.SaveChangesAsync();
        }
        public async Task<UserEnrollmentsViewModel> GetUserEnrollmentsAsync(int userId)
        {
            var enrollments = await _enrollmentRepo.GetUserEnrollmentsAsync(userId);
            var enrollmentDtos = _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);

            return new UserEnrollmentsViewModel
            {
                Enrollments = enrollmentDtos
            };
        }
    }
}
