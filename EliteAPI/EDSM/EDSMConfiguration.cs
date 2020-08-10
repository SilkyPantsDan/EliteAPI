using Newtonsoft.Json;

namespace EliteAPI.EDSM
{
    public class EDSMConfiguration
    {
        public EDSMConfiguration(string commanderName, string apiKey, string appName, string appVersion)
        {
            CommanderName = commanderName;
            ApiKey = apiKey;
            AppName = appName;
            AppVersion = appVersion;
        }
        [JsonProperty("fromSoftware")]
        public string AppName { get; internal set; }
        [JsonProperty("fromSoftwareVersion")]
        public string AppVersion { get; internal set; }
        [JsonProperty("apiKey")]
        public string ApiKey { get; internal set; }
        [JsonProperty("commanderName")]
        public string CommanderName { get; internal set; }
    }
}