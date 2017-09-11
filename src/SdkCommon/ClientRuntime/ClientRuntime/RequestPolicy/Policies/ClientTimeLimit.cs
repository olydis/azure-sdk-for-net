using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy.Policies
{

    public sealed class ClientTimeLimitPolicyFactory : IFactory
    {
        TimeSpan retry;
        TimeSpan overall;

        public ClientTimeLimitPolicyFactory(TimeSpan retry, TimeSpan overall)
        {
            this.retry = retry;
            this.overall = overall;
        }

        public IPolicy Create(PolicyNode node)
            => new ClientTimeLimitPolicy(node, retry, overall);

        private sealed class ClientTimeLimitPolicy : IPolicy
        {
            PolicyNode node;
            TimeSpan retry;
            TimeSpan overall;
            DateTime? operationStartTime;

            public ClientTimeLimitPolicy(PolicyNode node, TimeSpan retry, TimeSpan overall)
            {
                this.node = node;
                this.retry = retry;
                this.overall = overall;
                this.operationStartTime = null;
            }

            public async Task<HttpResponseMessage> SendAsync(Context ctx, HttpRequestMessage request)
            {
                if (!operationStartTime.HasValue)
                {
                    operationStartTime = DateTime.UtcNow;
                }
                if (DateTime.UtcNow - operationStartTime > overall)
                {
                    throw new Exception("overall operation time expired");
                }

                var response = await node.SendAsync(ctx.WithCancellationToken(new CancellationTokenSource(retry).Token), request);

                // TODO: cancel()
                // TODO: retry should happen if retry expired but overall did not
                return response;
            }
        }
    }
}
