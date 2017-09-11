using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    // The Policy interface represents a mutable Policy object created by a Factory. The object can mutate/process
    // the incoming HTTP reuqest and then forward it on to the next Policy object in the linked-list. The returned
    // Reponse goes backward through the linked-list for additional processing.
    public interface IPolicy
    {
        Task<HttpResponseMessage> SendAsync(Context ctx, HttpRequestMessage request);
    }
}
