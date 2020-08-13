using Newtonsoft.Json;

namespace EliteAPI.Event.Models
{
    public class MaterialDiscoveredEvent : EventBase
    {
        internal MaterialDiscoveredEvent() { }
        [JsonProperty("Category")]
        public string Category { get; internal set; }

        [JsonProperty("Name")]
        public string Name { get; internal set; }

        [JsonProperty("Name_Localised")]
        public string NameLocalised { get; internal set; }

        [JsonProperty("DiscoveryNumber")]
        public long DiscoveryNumber { get; internal set; }

        
    }
}