
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EliteAPI.Tests.Utilities {
    public class EchoHttpMessageHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = request.Content
                };
            });
        }
    }
}