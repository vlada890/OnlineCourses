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

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync(int? userId = null)
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
    }
}
