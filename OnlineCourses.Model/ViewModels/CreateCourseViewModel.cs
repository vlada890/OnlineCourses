using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.Model.ViewModels
{
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = "Course title is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
        [Display(Name = "Course Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Course description is required")]
        [StringLength(2000, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 2000 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 1000, ErrorMessage = "Duration must be between 1 and 1000 hours")]
        [Display(Name = "Duration (Hours)")]
        public int Duration { get; set; }

        [Display(Name = "Instructor")]
        public int InstructorId { get; set; } // The actual selected user ID

        public string? InstructorName { get; set; } // Optional
    }
}
