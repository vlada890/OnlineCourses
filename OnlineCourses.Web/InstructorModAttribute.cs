using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Web.Constants;

namespace OnlineCourses.Web.Filters
{
    /// <summary>
    /// Authorization filter that restricts access to Instructor, Admin, and SuperAdmin users
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class InstructorModAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public int Order { get; set; } = 2;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if user is logged in
            var userId = context.HttpContext.Session.GetInt32(SessionKeys.UserId);
            if (!userId.HasValue)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Check user role
            var userRole = context.HttpContext.Session.GetString(SessionKeys.UserRole);
            if (string.IsNullOrEmpty(userRole))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                return;
            }

            // Parse role and check if user is Instructor, Admin, or SuperAdmin
            if (Enum.TryParse<UserRole>(userRole, out var role))
            {
                if (role != UserRole.Instructor && role != UserRole.Admin && role != UserRole.SuperAdmin)
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                    return;
                }
            }
            else
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                return;
            }
        }
    }
}