using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    public interface IRequestPolicy
    {
        Task<HttpResponseMessage> SendAsync(Context context, HttpRequestMessage request);
    }
}
