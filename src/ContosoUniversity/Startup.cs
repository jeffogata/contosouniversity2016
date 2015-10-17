namespace ContosoUniversity
{
    using System;
    using DataAccess;
    using Infrastructure;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Hosting;
    using Microsoft.AspNet.Http;
    using Microsoft.AspNet.Mvc.Razor;
    using Microsoft.Data.Entity;
    using Microsoft.Dnx.Runtime;
    using Microsoft.Framework.Configuration;
    using Microsoft.Framework.DependencyInjection;
    using Models;

    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ContosoUniversityContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services
                .AddMvc();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Clear();
                options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            //{
            //    // Add Error handling middleware which catches all application specific errors and
            //    // send the request to the following path or controller action.
            //    app.UseExceptionHandler("/Home/Error");
            //}

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

            //app.Run(async context =>
            //{
            //    Department department = null;

            //    //using (var db = context.ApplicationServices.GetRequiredService<ContosoUniversityContext>())
            //    //{
            //    //    //department = await db.Departments.FirstOrDefaultAsync();
            //    //}

            //    //await
            //    //    context.Response.WriteAsync(
            //    //        $"Hello {department.Name}! {Configuration["Data:DefaultConnection:ConnectionString"]}");
            //    await
            //        context.Response.WriteAsync(
            //            $"Hello {Configuration["Data:DefaultConnection:ConnectionString"]}");
            //});
        }
    }
}