using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourses.Model.ViewModels
{
    public class CourseDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }

        public int InstructorId { get; set; }
        public string InstructorName { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 1000, ErrorMessage = "Duration must be between 1 and 1000 hours")]
        [Display(Name = "Duration (Hours)")]
        public int Duration { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
