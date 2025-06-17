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

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 1000, ErrorMessage = "Duration must be between 1 and 1000 hours")]
        [Display(Name = "Duration (Hours)")]
        public int Duration { get; set; }

        [Display(Name = "Instructor")]
        public int InstructorId { get; set; }  

        public string? InstructorName { get; set; }  // Optional

        public int EnrollmentCount { get; set; }
    }
}
