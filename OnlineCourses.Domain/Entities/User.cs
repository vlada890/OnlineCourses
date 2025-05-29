using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public UserRole Role { get; set; } = UserRole.Student;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
        public enum UserRole
    {
        [Display(Name = "Student")]
        Student = 1,

        [Display(Name = "Instructor")]
        Instructor = 2,

        [Display(Name = "Administrator")]
        Admin = 3,

        [Display(Name = "Super Administrator")]
        SuperAdmin = 4
    }
}
