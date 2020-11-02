using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AdventureWorks.Web
{
    /// <summary>
    /// Entry point class
    /// </summary>
    public class Program
    {
        #region Members

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        #endregion

        #region Methods

        /// <summary>
        /// Entry point method
        /// </summary>
        /// <param name="args">Arguments</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        #endregion
    }
}
