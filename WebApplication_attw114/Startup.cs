using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_attw114
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews();//default
            services.AddMvc();
            /*services.AddCors(options =>
            {
                options.AddPolicy("AllowFromAll",
                    builder => builder
                    .WithMethods("GET", "POST")
                    .AllowAnyOrigin()
                    .AllowAnyHeader());
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // set PathBase from header if nginx strips prefix
            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.TryGetValue("X-Forwarded-Prefix", out StringValues prefix) &&
                    !StringValues.IsNullOrEmpty(prefix))
                {
                    context.Request.PathBase = prefix.ToString().TrimEnd('/');
                }
                await next();
            });

            app.UsePathBase("/onduty"); // keep or remove based on chosen approach

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
            //app.UseCors("AllowFromAll");
        }
    }
}
