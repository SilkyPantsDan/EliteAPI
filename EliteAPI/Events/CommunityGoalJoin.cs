namespace EliteAPI.Events
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    public partial class CommunityGoalJoinInfo : IEvent
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; internal set; }
        [JsonProperty("event")]
        public string Event { get; internal set; }
        [JsonProperty("Name")]
        public string Name { get; internal set; }
        [JsonProperty("System")]
        public string System { get; internal set; }
    }
    public partial class CommunityGoalJoinInfo
    {
        internal static CommunityGoalJoinInfo Process(string json, EliteDangerousAPI api) => api.Events.InvokeCommunityGoalJoinEvent(JsonConvert.DeserializeObject<CommunityGoalJoinInfo>(json, EliteAPI.Events.CommunityGoalJoinConverter.Settings));
    }
    
    internal static class CommunityGoalJoinConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore, MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
