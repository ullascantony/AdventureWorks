using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using AdventureWorks.Domain.Interfaces;
using AdventureWorks.Repository;

namespace AdventureWorks.Web
{
    public class Startup
    {
        #region Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Dependency services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // IIS options
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = true;
            });

            // Make configuration available in the services collection
            services.AddSingleton(Configuration);

            // Add custom services
            services
                .AddTransient<IDatabaseContext, DatabaseContext>()
                .AddTransient<IDataAccess, DataAccess>()
                .AddTransient<IPersonRepository, PersonRepository>()
                .AddTransient<IEmployeeRepository, EmployeeRepository>();

            // Add services for controllers with JSON serialization options
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Hosting environment</param>
        /// <param name="loggerFactory">Logger factory</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts(); // The default HSTS value is 30 days.
            }

            // Enable default HTTPS redirection when available
            app.UseHttpsRedirection();

            // Enable options to serve static files like HTML, CSS & JS
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // Add URL based routing capabilities
            app.UseRouting();

            // Enable authorization capabilities
            app.UseAuthorization();

            // Enable options to use the default served file
            app.UseDefaultFiles();

            // Add endpoints to controller actions
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        #endregion
    }
}
