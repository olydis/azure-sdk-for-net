using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{

    // MethodFactoryMarker returns a special marker Factory object. When Pipeline's Do method is called, any
    // MethodMarkerFactory object is replaced with the specified methodFactory object. If nil is passed fro Do's
    // methodFactory parameter, then the MethodFactoryMarker is ignored as the linked-list of Policy objects is created.
    public sealed class MethodFactoryMarker : IFactory
    {
        public MethodFactoryMarker Instance { get; private set; } = new MethodFactoryMarker();

        private MethodFactoryMarker() { }

        public IPolicy Create(PolicyNode node)
        {
            throw new InvalidOperationException("methodFactoryMarker policy should have been replaced with a method policy");
        }
    }
}
