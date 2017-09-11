using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    public class HttpClientSender : IHttpSender
    {
        private readonly HttpClient client;

        public HttpClientSender() : this(new HttpClient()) { }

        public HttpClientSender(HttpClient client)
            => this.client = client;

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
    }
}
