using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeJungle.Entities;
using RecipeJungle.Services;

namespace Tasky.Infrastructure
{
    public class RecipePageFilter : IPageFilter
    {

        private readonly IServiceProvider serviceProvider;
        private readonly IUserService userService;

        public RecipePageFilter(IServiceProvider serviceProvider, IUserService userService)
        {
            this.serviceProvider = serviceProvider;
            this.userService = userService;
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {

        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            string authHeader = context.HttpContext.Request.Cookies["auth_Recipe"];
            if (string.IsNullOrWhiteSpace(authHeader))
            {
                context.Result = new RedirectResult("/login");
                return;
            }

            var token = authHeader;

            User user = userService.FindByToken(token);
            if (user == null)
            {
                context.Result = new RedirectResult("/login");
                return;
            }

            context.HttpContext.Items["user"] = user;
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }
    }
}