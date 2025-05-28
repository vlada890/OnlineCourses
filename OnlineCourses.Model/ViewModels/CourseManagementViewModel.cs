namespace OnlineCourses.Model.ViewModels
{
    public class CourseManagementViewModel
    {
        public IEnumerable<CourseManagementDto> Courses { get; set; }
        public int TotalCourses { get; set; }
        public int TotalEnrollments { get; set; }
    }

    public class CourseManagementDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public int EnrollmentCount { get; set; }
        public DateTime? LastEnrollment { get; set; }
    }
}
