using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
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
        {
            return new LoggingPolicy(node, logger);
        }

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
        {
            return new TransientFailure(node, delay, maxDelay, maxAttempts);
        }

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

            private static long pow(long number, int exponent)
            {
                long result = 1;
                for (int n = 0; n < exponent; n++)
                {
                    result *= number;
                }
                return result;
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
                    var delay = TimeSpan.FromTicks(this.delay.Ticks * ((pow(2, attempts) - 1) / 2));

                    if (delay > this.maxDelay)
                    {
                        delay = this.maxDelay;
                    }
                    await Task.Delay(delay, ctx.CancellationToken);
                }
                return response;
            }
        }
    }

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
        {
            return new ClientTimeLimitPolicy(node, retry, overall);
        }

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
                    operationStartTime = DateTime.Now;
                }
                if (DateTime.Now - operationStartTime > overall)
                {
                    throw new Exception("overall operation time expired");
                }
                
                // ctx, cancel:= context.WithTimeout(ctx, p.retry)

                var response = await node.SendAsync(ctx, request);

                // TODO: cancel()
                // TODO: retry should happen if retry expired but overall did not
                return response;
            }
        }
    }
}
