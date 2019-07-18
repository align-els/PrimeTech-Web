using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RecipeJungle.Contexts;
using RecipeJungle.Helpers;
using RecipeJungle.Middlewares;
using RecipeJungle.Services;
using Tasky.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace RecipeJungle
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddDbContext<RecipeContext>(opt =>
                opt.UseInMemoryDatabase("Recipe")
                //opt.UseSqlite("Data Source=database/dd.db;")
            );
            services.AddCors();
            services.AddScoped<RecipePageFilter>();
            services.AddMvc();
            services.AddOptions();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            Console.WriteLine(appSettingsSection.Path);
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();


            byte[] key = Encoding.ASCII.GetBytes(appSettings.ToBeOrNotToBe);
            _ = services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                }
            });

            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else {
                //app.UseExceptionHandler("/error");
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseMiddleware<ActionFailureHandler>();

            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller}/{action}");
            });

            app.UseAuthentication();
            app.UseStatusCodePages();

            app.UseStaticFiles();
        }
    }
}
