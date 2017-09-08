using Microsoft.Rest.TransientFaultHandling;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    public interface IRetryStrategy
    {
        RetryCondition 
    }

    public static class RetryPolicy
    {
        public static IRequestPolicy Create(IRequestPolicy next, IRetryStrategy strategy)
            => new Policy(
                next ?? throw new ArgumentNullException(nameof(next)),
                strategy ?? throw new ArgumentNullException(nameof(strategy))
            );

        private class Policy : IRequestPolicy
        {
            private readonly IRequestPolicy next;
            private readonly IRetryStrategy strategy;

            public Policy(IRequestPolicy next, IRetryStrategy strategy)
            {
                this.next = next;
                this.strategy = strategy;
            }

            public async Task<HttpResponseMessage> SendAsync(Context context, HttpRequestMessage request)
            {
                // TODO: make serious
                Console.WriteLine("Sending request");
                var result = await next.SendAsync(context, request);
                Console.WriteLine("Received response");
                return result;
            }
        }
    }
}
