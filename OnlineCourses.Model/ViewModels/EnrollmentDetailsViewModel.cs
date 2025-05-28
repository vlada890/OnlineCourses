namespace OnlineCourses.Model.ViewModels
{
    public class EnrollmentDetailsViewModel
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; }
    }
}
