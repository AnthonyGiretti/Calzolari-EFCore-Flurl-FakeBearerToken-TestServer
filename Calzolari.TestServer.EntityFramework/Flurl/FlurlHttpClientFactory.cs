using System.Net.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Calzolari.TestServer.EntityFramework.Flurl
{
    public class FlurlHttpClientFactory<TFactory, TStartup> : DefaultHttpClientFactory
        where TStartup : class
        where TFactory : WebApplicationFactory<TStartup>
    {
        private readonly TFactory _factory;

        public FlurlHttpClientFactory(TFactory factory)
        {
            _factory = factory;
        }

        // override to customize how HttpClient is created/configured
        public override HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return _factory.CreateClient();
        }
    }
}