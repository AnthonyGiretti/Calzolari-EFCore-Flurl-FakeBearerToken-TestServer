using Microsoft.Extensions.DependencyInjection;
using WebMotions.Fake.Authentication.JwtBearer;

namespace Calzolari.TestServer.EntityFramework.FakeBearerToken
{
    public static class IServiceCollectionExtensions
    {
        public static void AddFakeBearerToken(this IServiceCollection services)
        {
            services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme)
                    .AddFakeJwtBearer();
        }
    }
}