using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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

            var requestBodyStream = new MemoryStream();
            var originalRequestBody = context.Request.Body;

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var url = UriHelper.GetDisplayUrl(context.Request);
            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
            Console.Error.WriteLine($"[DEBUG-LOG] {context.Request.Method} {url} {requestBodyText}");

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;

            try {
                await next(context);
                context.Request.Body = originalRequestBody;
            }
            catch (ActionFailedException e) {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 420;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(e.Message));
            }
        }
    }
}
