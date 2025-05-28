using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineCourses.Web.Constants;

namespace OnlineCourses.Web.Filters
{
    public class AuthorizeSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (!userId.HasValue)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}