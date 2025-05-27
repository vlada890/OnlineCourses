using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Web.Constants;

namespace OnlineCourses.Web.Filters
{
    /// <summary>
    /// Authorization filter that restricts access based on specified user roles
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RequireRoleAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        private readonly UserRole[] _allowedRoles;
        public int Order { get; set; } = 2; // Executes after AdminMod if both are present

        public RequireRoleAttribute(params UserRole[] allowedRoles)
        {
            _allowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
        }

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

            // Parse role and check if user has required role
            if (Enum.TryParse<UserRole>(userRole, out var role))
            {
                if (!_allowedRoles.Contains(role))
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