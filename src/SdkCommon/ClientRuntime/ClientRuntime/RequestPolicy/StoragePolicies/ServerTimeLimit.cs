using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy.StoragePolicies
{
    public sealed class ServerTimeLimitPolicyFactory : IFactory
    {
        int serverPerRequestTimeout;

        public ServerTimeLimitPolicyFactory(int serverPerRequestTimeout)
        {
            this.serverPerRequestTimeout = serverPerRequestTimeout;
        }

        public IPolicy Create(PolicyNode node)
            => new ServerTimeLimitPolicy(node, serverPerRequestTimeout);

        private sealed class ServerTimeLimitPolicy : IPolicy
        {
            PolicyNode node;
            int serverPerRequestTimeout;

            public ServerTimeLimitPolicy(PolicyNode node, int serverPerRequestTimeout)
            {
                this.node = node;
                this.serverPerRequestTimeout = serverPerRequestTimeout;
            }

            public Task<HttpResponseMessage> SendAsync(Context ctx, HttpRequestMessage request)
            {
                var queryToAppend = $"timeout={serverPerRequestTimeout}";
                var q = new UriBuilder(request.RequestUri.Query);

                if (q.Query != null && q.Query.Length > 1)
                    q.Query = q.Query.Substring(1) + "&" + queryToAppend;
                else
                    q.Query = queryToAppend;

                request.RequestUri = q.Uri;
                return node.SendAsync(ctx, request);
            }
        }
    }
}
