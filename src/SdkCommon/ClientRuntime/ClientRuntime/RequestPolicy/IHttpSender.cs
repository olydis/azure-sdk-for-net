using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    // The HTTPSender interface represents an object that know how to send an HTTP request over the wire (returning an HTTP response).
    // Go's standard http.Client object satisfies this interface. But you can define your own type that implements this interface
    // to inject faults or to used a custom-configured http.Client object.
    public interface IHttpSender
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
