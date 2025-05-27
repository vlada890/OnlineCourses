using AutoMapper;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Model.ViewModels;

namespace OnlineCourses.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to DTO
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.Instructor, opt => opt.MapFrom(src => src.Instructor.FullName));

            CreateMap<Course, CourseDetailsViewModel>()
                .ForMember(dest => dest.Instructor, opt => opt.MapFrom(src => src.Instructor.FullName));

            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
                .ForMember(dest => dest.CourseInstructor, opt => opt.MapFrom(src => src.Course.Instructor.FullName));

            // ViewModel to Domain
            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
