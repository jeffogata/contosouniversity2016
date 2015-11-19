namespace ContosoUniversity
{
    using DataAccess;
    using Infrastructure;
    using Mapping;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Hosting;
    using Microsoft.AspNet.Http;
    using Microsoft.AspNet.Mvc.Razor;
    using Microsoft.Data.Entity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.PlatformAbstractions;
    using Serilog;

    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            Log.Logger = new LoggerConfiguration()
#if DNXCORE50
                .WriteTo.TextWriter(Console.Out)
#else
                .WriteTo.Trace()
#endif
                .CreateLogger();


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
                .AddMvc(options => { options.Conventions.Add(new FeatureApplicationModelConvention()); });

            services
                .AddMediator();

            services
                .Configure<RazorViewEngineOptions>(options =>
                {
                    options.ViewLocationExpanders.Clear();
                    options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddSerilog();
            //loggerFactory.MinimumLevel = LogLevel.Verbose;  // corresponds to Serilog's Debug

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.

                // todo: add error page
                app.UseExceptionHandler("/Home/Error");
            }

            // bootstrap AutoMapper
            app.ConfigureAutoMapper();

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
            
            app.Run(async context => { await context.Response.WriteAsync("<h1>Not Found</h1>"); });
        }
    }
}