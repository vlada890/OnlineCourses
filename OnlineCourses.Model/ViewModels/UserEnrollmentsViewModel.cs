using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourses.Model.ViewModels
{
    public class UserEnrollmentsViewModel
    {
        public IEnumerable<EnrollmentDto> Enrollments { get; set; }

    }
    public class EnrollmentDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseInstructor { get; set; }
        public DateTime EnrolledOn { get; set; }
    }
}
