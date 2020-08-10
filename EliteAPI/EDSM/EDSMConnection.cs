using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EliteAPI.EDSM
{
    public class EDSMConnection
    {
        private readonly string apiBase = "https://www.edsm.net";
        private readonly HttpClient apiClient;

        EDSMConnection()
        {
            apiClient = new HttpClient
            {
                BaseAddress = new Uri(apiBase),
                MaxResponseContentBufferSize = 256000
            };
        }
        

        /// <summary>
        /// Executes a generic EDSM API request.
        /// </summary>
        /// <param name="request">The HTTP Request to process.</param>
        /// <returns></returns>
        private async Task<T> SendRequestMessage<T>(HttpRequestMessage request)
        {
            var response = await apiClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"CONTENT FROM {request.RequestUri.ToString()}");
                Console.WriteLine(content);
                Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                return JsonConvert.DeserializeObject<T>(content);
            }

            return default;
        }

        public async Task<List<string>> GetDiscardedEventTypes() 
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("/api-journal-v1/discard"),
                Method = HttpMethod.Get
            };

            return await SendRequestMessage<List<string>>(request);
        }
    }
}
