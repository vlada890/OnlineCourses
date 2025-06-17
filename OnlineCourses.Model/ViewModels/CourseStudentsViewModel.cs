using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.Model.ViewModels
{
    public class CourseStudentsViewModel
    {
        public int CourseId { get; set; }

        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }

        [Display(Name = "Course Description")]
        public string CourseDescription { get; set; }

        [Display(Name = "Instructor")]
        public string InstructorName { get; set; }

        [Display(Name = "Total Students")]
        public int TotalStudents => Enrollments?.Count ?? 0;

        public List<EnrollmentDetailsViewModel> Enrollments { get; set; } = new List<EnrollmentDetailsViewModel>();
    }
}