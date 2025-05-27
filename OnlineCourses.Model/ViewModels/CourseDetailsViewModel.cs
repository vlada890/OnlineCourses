using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace OnlineCourses.Model.ViewModels
{
    public class CourseDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsEnrolled { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        [Display(Name = "Price ($)")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        [Display(Name = "Course Image URL")]
        public string ImageUrl { get; set; }

        // This will be set from session, not from form
        public int InstructorId { get; set; }
    }
}
