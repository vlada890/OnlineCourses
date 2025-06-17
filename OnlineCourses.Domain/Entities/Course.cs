using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public int InstructorId { get; set; }
        public User Instructor { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        [Required]
        [Range(1, 1000)]
        public int Duration { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
