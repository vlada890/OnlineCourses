using System.ComponentModel.DataAnnotations;
using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Model.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public UserRole Role { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
