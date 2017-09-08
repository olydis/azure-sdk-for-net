using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    public static class LoggingPolicy
    {
        public static IRequestPolicy Create(IRequestPolicy next)
            => new Policy(next ?? throw new ArgumentNullException(nameof(next)));

        private class Policy : IRequestPolicy
        {
            private readonly IRequestPolicy next;

            public Policy(IRequestPolicy next)
                => this.next = next;

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
