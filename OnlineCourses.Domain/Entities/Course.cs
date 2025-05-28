namespace OnlineCourses.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int InstructorId { get; set; }
        public User Instructor { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public int Duration { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
