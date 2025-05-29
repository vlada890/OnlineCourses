using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Model.ViewModels
{
    public class UserManagementViewModel
    {
        public IEnumerable<UserDto> Users { get; set; }
        public UserRole? FilterRole { get; set; }
        public int TotalUsers { get; set; }
        public int StudentsCount { get; set; }
        public int InstructorsCount { get; set; }
        public int AdminsCount { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int EnrollmentCount { get; set; }
    }
}
