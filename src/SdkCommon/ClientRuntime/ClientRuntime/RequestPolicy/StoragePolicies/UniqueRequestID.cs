using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy.StoragePolicies
{
    public sealed class UniqueRequestIDPolicyFactory : IFactory
    {
        public UniqueRequestIDPolicyFactory() { }

        public IPolicy Create(PolicyNode node)
            => new UniqueRequestIDPolicy(node);

        private sealed class UniqueRequestIDPolicy : IPolicy
        {
            PolicyNode node;

            public UniqueRequestIDPolicy(PolicyNode node)
            {
                this.node = node;
            }

            public Task<HttpResponseMessage> SendAsync(Context ctx, HttpRequestMessage request)
            {
                var newUUID = Guid.NewGuid();
                request.Headers.Add("x-ms-client-request-id", newUUID.ToString());
                return node.SendAsync(ctx, request);
            }
        }
    }
}
