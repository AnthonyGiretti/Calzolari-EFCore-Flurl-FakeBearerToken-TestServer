using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Flurl.Http;
using Microsoft.Net.Http.Headers;

namespace Calzolari.TestServer.EntityFramework.Flurl
{
    public static class FlurlExtensions
    {
        public static IFlurlRequest Route(this IFlurlRequest flurlRequest, string route)
        {
            return flurlRequest.AppendPathSegment(route);
        }

        public static IFlurlRequest Headers(this IFlurlRequest flurlRequest, IDictionary<string, IEnumerable<string>> headers)
        {
            if (headers != null && headers.Any())
            {
                foreach (var kvp in headers)
                {
                    flurlRequest.Client
                        .HttpClient
                        .DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                }
            }

            return flurlRequest;
        }

        public static IFlurlRequest Token(this IFlurlRequest flurlRequest, object token)
        {
            flurlRequest
                .Client
                .HttpClient
                .DefaultRequestHeaders.Add(HeaderNames.Authorization, token.ToString());

            return flurlRequest;
        }

        public static IFlurlRequest FakeToken(this IFlurlRequest flurlRequest, object token)
        {
            flurlRequest
                .Client
                .HttpClient
                .SetFakeBearerToken(token);

            return flurlRequest;
        }

        public static IFlurlRequest CleanHeaders(this IFlurlRequest flurlRequest)
        {
            flurlRequest
                .Client
                .HttpClient
                .DefaultRequestHeaders
                .Clear();

            return flurlRequest;
        }
    }
}