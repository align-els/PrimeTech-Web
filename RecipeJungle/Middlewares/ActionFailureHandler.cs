using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RecipeJungle.Exceptions;

namespace RecipeJungle.Middlewares
{
    public class ActionFailureHandler {
        private readonly RequestDelegate next;

        public ActionFailureHandler(RequestDelegate next) {
            this.next = next;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await next(context);
            }
            catch (ActionFailedException e) {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 420;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(e.Message));
            }
        }
    }
}
