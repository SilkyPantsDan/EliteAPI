
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EliteAPI.EDSM.Journal
{
    public class EDSMJournalEntryResponse
    {
        [JsonProperty("msgnum")]
        public long MessageStatus { get; internal set; }

        [JsonProperty("msg")]
        public string Message { get; internal set; }

        [JsonProperty("events")]
        public List<Event> Events { get; internal set; }
    }

    public class Event
    {
        [JsonProperty("msgnum")]
        public long MessageStatus { get; internal set; }

        [JsonProperty("msg")]
        public string Message { get; internal set; }
    }
}