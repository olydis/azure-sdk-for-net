using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    public static class HttpClientPolicy
    {
        public static IRequestPolicy Create()
            => Create(new HttpClient());
        public static IRequestPolicy Create(HttpClient client)
            => new Policy(client ?? throw new ArgumentNullException(nameof(client)));

        private class Policy : IRequestPolicy
        {
            private readonly HttpClient client;

            public Policy(HttpClient client)
                => this.client = client;

            public Task<HttpResponseMessage> SendAsync(Context context, HttpRequestMessage request)
                => client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.CancellationToken);
        }
    }
}
