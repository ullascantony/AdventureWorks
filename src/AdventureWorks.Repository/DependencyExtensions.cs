using Microsoft.Extensions.DependencyInjection;

using AdventureWorks.Domain.Interfaces;

namespace AdventureWorks.Repository
{
    /// <summary>
    /// Register all DI providers and services
    /// </summary>
    public static class DependencyExtensions
    {
        #region Methods

        public static IServiceCollection Register(this IServiceCollection services)
        {
            services
                .AddTransient<IDatabaseContext, DatabaseContext>()
                .AddTransient<IDataAccess, DataAccess>()
                .AddTransient<IPersonRepository, PersonRepository>()
                .AddTransient<IEmployeeRepository, EmployeeRepository>();

            return services;
        }

        #endregion
    }
}
