using AutoMapper;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.Data.Repositories;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Model.Common;
using OnlineCourses.Model.ViewModels;

namespace OnlineCourses.BusinessLogic.Services
{
    public class CourseService : ICourseService
    {

        private readonly ICourseRepository _courseRepo;
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IMapper _mapper;

        public CourseService(
            ICourseRepository courseRepo,
            IEnrollmentRepository enrollmentRepo,
            IMapper mapper)
        {
            _courseRepo = courseRepo;
            _enrollmentRepo = enrollmentRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync(int? userId = null)//, string searchTerm = null)
        {
            var courses = await _courseRepo.GetAllAsync();

            var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);

            if (userId.HasValue)
            {
                foreach (var course in courseDtos)
                {
                    course.IsEnrolled = await _enrollmentRepo.IsUserEnrolledAsync(userId.Value, course.Id);
                }
            }

            return courseDtos;
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

        public async Task<bool> EnrollAsync(int userId, int courseId)
        {
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
        public async Task<ServiceResult<Course>> CreateCourseAsync(CreateCourseViewModel model)
        {
            try
            {
                var course = _mapper.Map<Course>(model);
                await _courseRepo.AddAsync(course);
                var success = await _courseRepo.SaveChangesAsync();
        
                if (!success)
                    return ServiceResult<Course>.FailureResult("Failed to create course");
        
                return ServiceResult<Course>.SuccessResult(course, "Course created successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<Course>.FailureResult($"Error creating course: {ex.Message}");
            }
        }
        public async Task<ServiceResult<bool>> DeleteCourseAsync(int id)
        {
            try
            {
                var course = await _courseRepo.GetByIdAsync(id);
                if (course == null)
                    return ServiceResult<bool>.FailureResult("Course not found");
        
                await _courseRepo.DeleteAsync(id);
                var success = await _courseRepo.SaveChangesAsync();
        
                if (!success)
                    return ServiceResult<bool>.FailureResult("Failed to delete course");
        
                return ServiceResult<bool>.SuccessResult(true, "Course deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.FailureResult($"Error deleting course: {ex.Message}");
            }
        }
        public async Task<ServiceResult<List<Course>>> GetCoursesByInstructorAsync(int instructorId)
        {
            try
            {
                var courses = await _courseRepo.GetCoursesByInstructorAsync(instructorId);
                return ServiceResult<List<Course>>.SuccessResult(courses.ToList(), "Courses retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<List<Course>>.FailureResult($"Error retrieving instructor courses: {ex.Message}");
            }
        }
        public async Task<ServiceResult<Course>> UpdateCourseAsync(int id, CreateCourseViewModel model)
        {
            try
            {
                var course = await _courseRepo.GetByIdAsync(id);
                if (course == null)
                    return ServiceResult<Course>.FailureResult("Course not found");
        
                _mapper.Map(model, course); // Update course with new values
        
                _courseRepo.Update(course); // Assuming Update method exists
                var success = await _courseRepo.SaveChangesAsync();
        
                if (!success)
                    return ServiceResult<Course>.FailureResult("Failed to update course");
        
                return ServiceResult<Course>.SuccessResult(course, "Course updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<Course>.FailureResult($"Error updating course: {ex.Message}");
            }
        }
        public async Task<IEnumerable<EnrollmentDetailsViewModel>> GetCourseEnrollmentsAsync(int courseId)
        {
            var enrollments = await _enrollmentRepo.GetCourseEnrollmentsAsync(courseId);
            return _mapper.Map<IEnumerable<EnrollmentDetailsViewModel>>(enrollments);
        }
        public async Task<ServiceResult<List<CourseAdminViewModel>>> GetAllCoursesForAdminAsync()
        {
            try
            {
                var courses = await _courseRepo.GetAllAsync();
        
                // Project to view model (enrollment count etc.)
                var courseViewModels = new List<CourseAdminViewModel>();
        
                foreach (var course in courses)
                {
                    var enrollmentCount = await _enrollmentRepo.GetEnrollmentCountForCourseAsync(course.Id); //  this is a repo method
                    courseViewModels.Add(new CourseAdminViewModel
                    {
                        Id = course.Id,
                        Title = course.Title,
                        Description = course.Description,
                        Duration = course.Duration,
                        CreatedDate = course.CreatedOn,
                        EnrollmentCount = enrollmentCount
                    });
                }
        
                return ServiceResult<List<CourseAdminViewModel>>.SuccessResult(courseViewModels);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<CourseAdminViewModel>>.FailureResult($"Error retrieving admin courses: {ex.Message}");
            }
        }

    }
}
