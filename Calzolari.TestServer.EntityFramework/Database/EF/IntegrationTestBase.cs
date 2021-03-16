using System;
using Calzolari.TestServer.EntityFramework.Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace Calzolari.TestServer.EntityFramework.Database.EF
{
    public class IntegrationTestBase<TDbContext, TStartup> : IDisposable where TDbContext : DbContext
                                                                         where TStartup : class
                                                                 
    {
        protected TDbContext DbContext;
        protected readonly IFlurlRequest BASE_REQUEST = "http://localhost".AllowAnyHttpStatus();

        public IntegrationTestBase(WebApplicationFactory<TStartup> factory)
        {
            DbContext = factory.GetScopedDbContext<TDbContext, TStartup>();
            DbContext.Database.EnsureCreated();
        }

        public int Arrange(Action<TDbContext> arrangeFunc)
        {
            arrangeFunc(DbContext);
            return DbContext.SaveChanges();
        }

        public void Dispose()
        {
            BASE_REQUEST.CleanHeaders();
            DbContext.Database.EnsureDeleted();
        }
    }
}