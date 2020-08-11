using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EliteAPI.EDSM.Journal;
using Newtonsoft.Json;

namespace EliteAPI.EDSM
{
    public class EDSMConnection
    {
        private readonly string apiBase = "https://www.edsm.net";
        private readonly HttpClient apiClient;

        public EDSMConfiguration Configuration { get; internal set; }
        private List<string> DiscardEvents = new List<string>();

        public EDSMConnection(EDSMConfiguration configuration, HttpMessageHandler messageHandler = null)
        {
            Configuration = configuration;

            apiClient = new HttpClient(messageHandler ?? new HttpClientHandler())
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
        internal async Task<string> SendRequestMessage(HttpRequestMessage request)
        {
            var response = await apiClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return string.Empty;
        }
        

        /// <summary>
        /// Executes a generic EDSM API request.
        /// </summary>
        /// <param name="request">The HTTP Request to process.</param>
        /// <returns></returns>
        internal async Task<T> SendRequestMessage<T>(HttpRequestMessage request)
        {
            var response = await apiClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return default;
        }

        #region Journal

        public async Task<string> UploadJournalEvents(List<Events.EventBase> events) 
        {
            var filteredEvents = events.Where(x => !this.DiscardEvents.Contains(x.Event));
            var journalEvent = new EDSMJournalEntry(this.Configuration, filteredEvents);
            var json = JsonConvert.SerializeObject(journalEvent);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("/api-journal-v1", UriKind.Relative),
                Method = HttpMethod.Post,
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };

            var response = await SendRequestMessage(request);

            return response;
        }

        public async Task<List<string>> GetDiscardedEventTypes() 
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("/api-journal-v1/discard"),
                Method = HttpMethod.Get
            };

            this.DiscardEvents = await SendRequestMessage<List<string>>(request);

            return this.DiscardEvents;
        }

        #endregion
    }
}
