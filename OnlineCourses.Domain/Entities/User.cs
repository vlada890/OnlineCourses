using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(70)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.Student;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Course> CoursesCreated { get; set; } = new List<Course>();// For instructors
    }
}