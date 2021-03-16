using Calzolari.TestServer.EntityFramework.Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Calzolari.WebApi.Tests.Common
{
    public class DemoFactory : FlurlWebFactory<Startup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder().ConfigureWebHost((builder) =>
            {
                builder.UseStartup<TestStartup>();
            });
        }
    }
}