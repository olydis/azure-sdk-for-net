using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy.Policies
{
    public interface ILogger
    {
        void Log(string message);
    }

    public sealed class LoggingPolicyFactory : IFactory
    {
        ILogger logger;

        public LoggingPolicyFactory(ILogger logger)
        {
            this.logger = logger;
        }

        public IPolicy Create(PolicyNode node)
            => new LoggingPolicy(node, logger);

        private sealed class LoggingPolicy : IPolicy
        {
            PolicyNode node;
            ILogger logger;

            public LoggingPolicy(PolicyNode node, ILogger logger)
            {
                this.node = node;
                this.logger = logger;
            }

            public async Task<HttpResponseMessage> SendAsync(Context ctx, HttpRequestMessage request)
            {
                var start = DateTime.UtcNow;
                var id = ((start.Minute * 60 + start.Second) * 1000) + (start.Ticks / 1000);
                logger.Log($"-> {id} {0.0} {request.Method} {request.RequestUri}\n");

                var response = await node.SendAsync(ctx, request);
                    
                var end = DateTime.UtcNow;
                logger.Log($"<- {id} {(end - start).TotalSeconds} {request.Method} {request.RequestUri}\n");

                return response;
            }
        }
    }
}
