using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Data.Entity;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace ContosoUniversity
{
    using DataAccess;
    using Microsoft.AspNet.Hosting;
    using Microsoft.Dnx.Runtime;
    using Microsoft.Framework.Configuration;
    using Models;

    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ContosoUniversityContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));
        }

        public void Configure(IApplicationBuilder app)
        {
            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            app.Run(async (context) =>
            {
                Department department = null;

                using (var db = context.ApplicationServices.GetService<ContosoUniversityContext>())
                {
                    department = await db.Departments.FirstOrDefaultAsync();
                }

                await context.Response.WriteAsync($"Hello {department.Name}! {Configuration["Data:DefaultConnection:ConnectionString"]}");
            });
        }
    }
}
