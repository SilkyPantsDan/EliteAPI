namespace EliteAPI.Events
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    public partial class NpcCrewPaidWageInfo : IEvent
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; internal set; }
        [JsonProperty("event")]
        public string Event { get; internal set; }
        [JsonProperty("NpcCrewName")]
        public string NpcCrewName { get; internal set; }
        [JsonProperty("NpcCrewId")]
        public long NpcCrewId { get; internal set; }
        [JsonProperty("Amount")]
        public long Amount { get; internal set; }
    }
    public partial class NpcCrewPaidWageInfo
    {
        internal static NpcCrewPaidWageInfo Process(string json, EliteDangerousAPI api) => api.Events.InvokeNpcCrewPaidWageEvent(JsonConvert.DeserializeObject<NpcCrewPaidWageInfo>(json, EliteAPI.Events.NpcCrewPaidWageConverter.Settings));
    }
    
    internal static class NpcCrewPaidWageConverter
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
