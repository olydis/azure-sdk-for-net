using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    public sealed class ExponentialRetryOptions
    {
        public Uri SecondaryURL { get; private set; }
        public TimeSpan MaxOperationTime { get; private set; }
        public int MaxAttempts { get; private set; }
        public TimeSpan RetryDelay { get; private set; }
        public TimeSpan MaxDelay { get; private set; }

        public ExponentialRetryOptions(
            Uri secondaryURL,
            TimeSpan maxOperationTime,
            int maxAttempts,
            TimeSpan retryDelay,
            TimeSpan maxDelay)
        {
            this.SecondaryURL = secondaryURL;
            this.MaxOperationTime = maxOperationTime;
            this.MaxAttempts = maxAttempts;
            this.RetryDelay = retryDelay;
            this.MaxDelay = maxDelay;
        }
    }

    public sealed class ExponentialRetryPolicyFactory : IFactory
    {
        ExponentialRetryOptions o;

        public ExponentialRetryPolicyFactory(ExponentialRetryOptions o)
        {
            this.o = o;
        }

        public IPolicy Create(PolicyNode node)
            => new ExponentialRetryPolicy(node, o);

        private sealed class ExponentialRetryPolicy : IPolicy
        {
            PolicyNode node;
            ExponentialRetryOptions o;

            public ExponentialRetryPolicy(PolicyNode node, ExponentialRetryOptions o)
            {
                this.node = node;
                this.o = o;
            }

            public async Task<HttpResponseMessage> SendAsync(Context ctx, HttpRequestMessage request)
            {
                var operationEndTime = DateTime.Now + o.MaxOperationTime;

                Exception err = null;

                // We only consider retring against a secondary if we have a read request (GET/HEAD) AND theis policy has a Secondary URL it can use
                var considerSecondary = (request.Method == HttpMethod.Get || request.Method == HttpMethod.Head) && o.SecondaryURL.Host != "";

                // Exponential retry algorithm: ((2 ^ attempt) - 1) * delay * random(0.8, 1.2)
                // When to retry: connection failure or an HTTP status code of 500 or greater, except 501 and 505
                // If using a secondary:
                //    Even attempts go agsainst primary; odd attempts go against the secondary
                //    For a primary wait ((2 ^ (attempt / 2) - 1) * delay * random(0.8, 1.2)
                //    If secondary gets a 404, don't fail, retry but future retries are only against the primary
                //    When retrying against a secondary, ignore the retry count and wait (.1 second * random(0.8, 1.2))
                for (int attempt = 0; (attempt < o.MaxAttempts) && DateTime.Now < operationEndTime; attempt++)
                {
                    // Determine which endpoint to try. It's primary if there is no secondary or if it is an even attempt.
                    var tryingPrimary = !considerSecondary || (attempt % 2 == 0);

                    // Select the right URL endpoint & make the request
                    // TODO: Fix the endpoint

                    // Insert delay:
                    var delay = tryingPrimary
                        ? TimeSpan.FromTicks(o.RetryDelay.Ticks * ((Pow(2, attempt) - 1) / 2))
                        : TimeSpan.FromSeconds(1);

                    // .8 + [0, .1, .2, .3, or .4] = .8, .9, 1.0, 1.1, or 1.2
                    delay = TimeSpan.FromTicks(delay.Ticks * (8 + new Random().Next(5)) / 10);

                    if (delay > o.MaxDelay)
                    {
                        delay = o.MaxDelay;
                    }
                    await Task.Delay(delay, ctx.CancellationToken);

                    HttpResponseMessage response = null;
                    bool retry = false;
                    try
                    {
                        response = await node.SendAsync(ctx, request); // Make the request

                        // Retry if we got a response and the code is >= 500 (InternalServerError) but not 501 (not implemented) or 505 (HTTPVersionNotSupported)
                        retry = response != null &&
                           response.StatusCode >= HttpStatusCode.InternalServerError /*500*/ &&
                           response.StatusCode != HttpStatusCode.NotImplemented /* 501 */ &&
                           response.StatusCode != HttpStatusCode.HttpVersionNotSupported; /* 505 */
                    }
                    catch (Exception e)
                    {
                        err = e;
                        // If temporary or timeout; retry
                        //TODO:
                        //if (nerr, ok:= err.(net.Error); ok)
                        //{
                        //    retry = nerr.Temporary() || nerr.Timeout()
                        //}
                    }

                    if (!retry)
                    {
                        // We're not going to retry, return what we have
                        if (err != null)
                        {
                            throw err;
                        }
                        return response;
                    }

                    // If we get here, we're going to retry.
                    // If attempt was against the secondary & it returned a StatusNotFound (404), then the resource
                    // was not found. This may be due to replication delay. In this case, never try the secondary again for this operation.
                    if (!tryingPrimary && response?.StatusCode == HttpStatusCode.NotFound) /* 404 */
                    {
                        considerSecondary = false;
                    }
                }
                throw err; // Too many attempts; return the last error
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
