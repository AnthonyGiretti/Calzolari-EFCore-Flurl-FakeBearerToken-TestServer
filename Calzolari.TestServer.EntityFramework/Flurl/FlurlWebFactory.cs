using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Calzolari.TestServer.EntityFramework.Flurl
{
    public class FlurlWebFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public FlurlWebFactory()
        {
            FlurlHttp.Configure(settings => {
                settings.HttpClientFactory = new FlurlHttpClientFactory<FlurlWebFactory<TStartup>, TStartup>(this);
            });
        }
    }
}