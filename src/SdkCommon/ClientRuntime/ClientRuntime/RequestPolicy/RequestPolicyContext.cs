using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    public class Context
    {
        public static readonly Context None = new Context(CancellationToken.None);

        public CancellationToken CancellationToken { get; private set; }

        public Context(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }

        public Context WithNewCancellationToken(CancellationToken newCancellationToken) => new Context(newCancellationToken);

        public static implicit operator Context(CancellationToken ct) => new Context(ct);
    }
}
