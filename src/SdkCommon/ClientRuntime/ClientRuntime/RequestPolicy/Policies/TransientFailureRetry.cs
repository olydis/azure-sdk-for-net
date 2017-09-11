using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy.Policies
{
    public sealed class TransientFailureRetryPolicyFactory : IFactory
    {
        TimeSpan delay;
        TimeSpan maxDelay;
        int maxAttempts;

        public TransientFailureRetryPolicyFactory(TimeSpan delay, TimeSpan maxDelay, int maxAttempts)
        {
            this.delay = delay;
            this.maxDelay = maxDelay;
            this.maxAttempts = maxAttempts;
        }

        public IPolicy Create(PolicyNode node)
            => new TransientFailure(node, delay, maxDelay, maxAttempts);

        private sealed class TransientFailure : IPolicy
        {
            PolicyNode node;
            TimeSpan delay;
            TimeSpan maxDelay;
            int maxAttempts;

            public TransientFailure(PolicyNode node, TimeSpan delay, TimeSpan maxDelay, int maxAttempts)
            {
                this.node = node;
                this.delay = delay;
                this.maxDelay = maxDelay;
                this.maxAttempts = maxAttempts;
            }

            public async Task<HttpResponseMessage> SendAsync(Context ctx, HttpRequestMessage request)
            {
                HttpResponseMessage response = null;
                for (int attempts = 0; attempts < maxAttempts; attempts++)
                {
                    try
                    {
                        response = await node.SendAsync(ctx, request);

                        if (response != null)
                        {
                            // We got a response
                            if (response.StatusCode != HttpStatusCode.ServiceUnavailable) /*503*/
                            {
                                break; // If not 503, don't retry
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        // response is nil, let's look at the error
                        if (/* TODO: err transient? */ false)
                        {
                            // It is temporary; we'll retry
                        }
                        else
                        {
                            throw err;
                        }
                    }
                    // If we get here, delay & then retry
                    // TODO: really close the body here?
                    //response.Response().Body.Close()
                    var delay = TimeSpan.FromTicks(this.delay.Ticks * ((Pow(2, attempts) - 1) / 2));

                    if (delay > this.maxDelay)
                    {
                        delay = this.maxDelay;
                    }
                    await Task.Delay(delay, ctx.CancellationToken);
                }
                return response;
            }

            private static long Pow(long number, int exponent)
            {
                long result = 1;
                for (int n = 0; n < exponent; n++)
                {
                    result *= number;
                }
                return result;
            }
        }
    }
}
