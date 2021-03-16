using Calzolari.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Calzolari.TestServer.EntityFramework.Database.EF
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationTestDbContext<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            return services.AddIntegrationTestDbContext<TContext>("DataSource=IntegrationTestDb");
        }

        public static IServiceCollection AddIntegrationTestDbContext<TContext>(this IServiceCollection services, string dbName) where TContext : DbContext
        {
            services.Unregister<TContext>();
            services.AddDbContext<TContext>(opt => opt.UseSqlite($"DataSource={dbName}"));

            return services;
        }
    }
}