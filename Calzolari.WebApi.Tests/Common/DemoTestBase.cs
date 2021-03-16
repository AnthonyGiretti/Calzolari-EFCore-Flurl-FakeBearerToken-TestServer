using AutoFixture;
using Calzolari.TestServer.EntityFramework.Database.EF;
using Calzolari.WebApi.Database;
using Xunit;

namespace Calzolari.WebApi.Tests.Common
{
    [Collection("AssemblyFixture")]
    public class DemoTestBase : IntegrationTestBase<DemoDbContext, Startup>
    {
        protected readonly IFixture Fixture;

        public DemoTestBase(DemoFactory factory) : base(factory)
        {
            Fixture = new Fixture();
        }
    }
}