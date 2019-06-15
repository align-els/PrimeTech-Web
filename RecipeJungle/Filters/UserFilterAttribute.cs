using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using RecipeJungle.Entities;
using RecipeJungle.Services;

namespace RecipeJungle.Filters
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public sealed class UserFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("user"))
                return;

            object userParameter = context.ActionArguments["user"];

            ControllerBase controller = (ControllerBase)context.Controller;
            string username = controller.User.FindFirst(ClaimTypes.Name)?.Value;
            IServiceProvider serviceProvider = context.HttpContext.RequestServices.GetRequiredService<IServiceProvider>();

            IUserService userService = serviceProvider.GetRequiredService<IUserService>();
            User user = userService.FindByUserName(username);

            context.ActionArguments["user"] = user;
        }
    }

}
