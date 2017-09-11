using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    // The Pipeline interface represents an ordered list of Factory objects and an object implementing the HTTPSender interface.
    // You construct a Pipeline by calling the pipeline.NewPipeline function. To send an HTTP request, call pipeline.NewRequest
    // and then call Pipeline's Do method passing a context, the request, and a method-specific Factory (or nil). Passing a
    // method-specific Factory allows this one call to Do to inject a Policy into the linked-list. The policy is injected where
    // the MethodFactoryMarker (see the pipeline.MethodFactoryMarker function) is in the slice of Factory objects.
    //
    // When Do is called, the Pipeline object asks each Factory object to construct its Policy object and adds each Policy to a linked-list.
    // THen, Do sends the Context and Request through all the Policy objects. The final Policy object sends the request over the network
    // (via the HTTPSender object passed to NewPipeline) and the response is returned backwards through all the Policy objects.
    // Since Pipeline and Factory objects are immutable, you typically create 1 Pipeline object and reuse it to make many HTTP requests.
    public class Pipeline
    {
        public IEnumerable<IFactory> Factories { get; private set; }
        public IHttpSender Sender { get; private set; }

        public Pipeline(IEnumerable<IFactory> factories) : this(factories, new HttpClientSender()) { }

        // NewPipeline creates a new immutable Pipeline object from the slice of Factory objects and the specified HTTPSender.
        // If sender is nil, then a default (zero-value) http.Client is created for this Pipeline object.
        public Pipeline(IEnumerable<IFactory> factories, IHttpSender sender)
        {
            this.Factories = factories;
            this.Sender = sender;
        }

        // SendAsync is called for each and every HTTP request. It tells each Factory to create its own (mutable) Policy object
        // replacing a MethodFactoryMarker factory (if it exists) with the methodFactory passed in. Then, the Context and Request
        // are sent through the pipeline of Policy objects (which can transform the Request's URL/query parameters/headers) and
        // ultimately sends the transformed HTTP request over the network.
        public Task<HttpResponseMessage> SendAsync(Context ctx, IFactory methodFactory, HttpRequestMessage request)
        {
            // defer request.close()   ... so Dispose request afterwards? not sure that's expected by C# people but maybe required for "determinism" of GC work?
            return NewPolicies(methodFactory).SendAsync(ctx, request);
        }

        private IPolicy NewPolicies(IFactory methodFactory)
        {
            var node = new PolicyNode(this, null);
            node.Next = new LastPolicy(node, Sender);

            // Walk over the slice of Factory objects
            int markers = 0;

            foreach (var factory in Factories)
            {
                if (factory is MethodFactoryMarker)
                {
                    markers++;
       
                    if (markers > 1 )
                    {
                        throw new Exception("MethodFactoryMarker can only appear once in the pipeline");
                    }
                    if (methodFactory != null)
                    {
                        // Replace MethodFactoryMarker with passed-in methodFactory
                        node.Next = methodFactory.Create(node);
                    }
                }
                else 
                {
                    // Use the slice's Factory to construct its Policy
                    node.Next = factory.Create(node);
                }
            }

            // Each Factory has created its Policy
            if (markers == 0 && methodFactory != null)
            {
                throw new Exception("Non-nil methodFactory requires MethodFactoryMarker in the pipeline");
            }
            return node.Next; // Return head of the Policy object linked-list
        }

        private sealed class LastPolicy : IPolicy
        {
            public PolicyNode Node { get; private set; }
            public IHttpSender Sender { get; private set; }

            public LastPolicy(PolicyNode node, IHttpSender sender)
            {
                this.Node = node;
                this.Sender = sender;
            }

            public async Task<HttpResponseMessage> SendAsync(Context ctx, HttpRequestMessage request)
            {
                // seek to the beginning in case this is a retry
                if (request.Content is StreamContent sc)
                {
                    // CAN/SHOULD WE DO THIS? how to "reset" an HttpContent?
                    (await sc.ReadAsStreamAsync()).Seek(0, System.IO.SeekOrigin.Begin);
                }

                // request.Request = request.WithContext(ctx)

                return await Sender.SendAsync(request, ctx.CancellationToken);
            }
        }
    }
}
