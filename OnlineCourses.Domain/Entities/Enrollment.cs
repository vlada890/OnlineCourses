namespace OnlineCourses.Domain.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrolledOn { get; set; }

        public User User { get; set; }
        public Course Course { get; set; }
    }
}