using AutoMapper;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Model.ViewModels;

namespace OnlineCourses.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to DTO mappings
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Instructor, opt => opt.MapFrom(src => src.Instructor.FullName));
        
        CreateMap<Course, CourseDetailsViewModel>()
            .ForMember(dest => dest.Instructor, opt => opt.MapFrom(src => src.Instructor.FullName));

            CreateMap<Course, CourseManagementDto>()
                .ForMember(dest => dest.EnrollmentCount, opt => opt.MapFrom(src => src.Enrollments.Count));
        
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.EnrollmentCount, opt => opt.MapFrom(src => src.Enrollments.Count));
        
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
                .ForMember(dest => dest.CourseInstructor, opt => opt.MapFrom(src => src.Course.Instructor));
        
            // ViewModel to Domain mappings
            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)) ;
        
            CreateMap<CreateCourseViewModel, Course>();
            CreateMap<EditCourseViewModel, Course>();
        
            // Domain to ViewModel mappings
            CreateMap<User, EditUserViewModel>();
            CreateMap<EditUserViewModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Enrollments, opt => opt.Ignore());
        
            CreateMap<Course, EditCourseViewModel>();
            CreateMap<Course, CourseAdminViewModel>()
            .ForMember(dest => dest.EnrollmentCount, opt => opt.MapFrom(src => src.Enrollments.Count))
            .ForMember(dest => dest.Duration, opt => opt.Ignore())    // since Course doesn't have Duration
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
        }
    }
}
