using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Calzolari.TestServer.EntityFramework.Database.EF
{
    public static class IServiceProviderExtensions
    {
        public static TDbContext GetScopedDbContext<TDbContext,  TStartup>(this WebApplicationFactory<TStartup> factory) 
            where TStartup : class

            where TDbContext : DbContext
        {
            return factory.Services
                          .CreateScope()
                          .ServiceProvider
                          .GetRequiredService<TDbContext>();
        }
    }
}
