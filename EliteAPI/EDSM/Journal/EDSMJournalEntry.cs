using System.Collections.Generic;
using Newtonsoft.Json;

namespace EliteAPI.EDSM.Journal
{
    public class EDSMJournalEntry
    {
        public EDSMJournalEntry(EDSMConfiguration configuration, IEnumerable<Events.EventBase> journalEvents)
        {
            Events = journalEvents;
            CommanderName = configuration.CommanderName;
            ApiKey = configuration.ApiKey;
            AppName = configuration.AppName;
            AppVersion = configuration.AppVersion;
        }
        [JsonProperty("fromSoftware")]
        public string AppName { get; internal set; }
        [JsonProperty("fromSoftwareVersion")]
        public string AppVersion { get; internal set; }
        [JsonProperty("apiKey")]
        public string ApiKey { get; internal set; }
        [JsonProperty("commanderName")]
        public string CommanderName { get; internal set; }
        [JsonProperty("message")]
        public IEnumerable<Events.EventBase> Events { get; internal set; }
    }
}