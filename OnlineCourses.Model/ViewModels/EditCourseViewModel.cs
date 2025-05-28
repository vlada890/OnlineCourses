using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.Model.ViewModels
{
    public class EditCourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course title is required")]
        [Display(Name = "Course Title")]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Course description is required")]
        [Display(Name = "Description")]
        [StringLength(1000, MinimumLength = 10)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Instructor is required")]
        [Display(Name = "Instructor")]
        public int InstructorId { get; set; }  

        public string InstructorName { get; set; }  // Optional

        public int EnrollmentCount { get; set; }
    }
}
