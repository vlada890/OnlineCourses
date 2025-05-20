using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.Model.ViewModels
{
    public class EnrollmentViewModel
    {
        [Required]
        public int CourseId { get; set; }
    }
}
