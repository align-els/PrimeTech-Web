using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeJungle.Contexts;
using RecipeJungle.Services;

namespace RecipeJungle
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddDbContext<RecipeContext>(opt =>
                opt.UseInMemoryDatabase("test")
            );

            services.AddMvc().AddMvcOptions(options =>
            {
            });

            services.AddScoped<IRecipeService, RecipeService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else {
                //app.UseExceptionHandler("/error");
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller}/{action}");
            });

            app.UseAuthentication();
            app.UseStatusCodePages();

            app.UseStaticFiles();
        }
    }
}
