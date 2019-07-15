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

            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authHeader))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401
                };
                return;
            }

            var token = authHeader;

            IServiceProvider serviceProvider = context.HttpContext.RequestServices.GetRequiredService<IServiceProvider>();
            IUserService userService = serviceProvider.GetRequiredService<IUserService>();
            User user = userService.FindByToken(token);
            if (user == null)
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401
                };
                return;
            }

            context.ActionArguments["user"] = user;
        }
    }

}
