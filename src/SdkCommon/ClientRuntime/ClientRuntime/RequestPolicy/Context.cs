using System.Threading;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    public class Context
    {
        public CancellationToken CancellationToken { get; private set; }

        public Context() : this(CancellationToken.None) { }

        public Context(CancellationToken cancellationToken) => CancellationToken = cancellationToken;

        public Context WithCancellationToken(CancellationToken token)
            => new Context(CancellationTokenSource.CreateLinkedTokenSource(CancellationToken, token).Token);
    }
}
