using System;
using System.Collections.Generic;

namespace EliteAPI.EDSM.Journal
{
    interface IEDSMGameStatus
    {
        long? SystemId { get; set; }
        string SystemName { get; set; }
        List<float> SystemCoordinates { get; set; }
        long? StationId { get; set; }
        string StationName { get; set; }
        long? ShipId { get; set; }
    }

    public class EDSMJournalEntry : Dictionary<string, dynamic>, IEDSMGameStatus, Events.IEvent
    {
        public long? SystemId { get => this["_systemAddress"]; set => this["_systemAddress"] = value; }
        public string SystemName { get => this["_systemName"]; set => this["_systemName"] = value; }
        public List<float> SystemCoordinates { get => this["_systemCoordinates"]; set => this["_systemCoordinates"] = value; }
        public long? StationId { get => this["_marketId"]; set => this["_marketId"] = value; }
        public string StationName { get => this["_stationName"]; set => this["_stationName"] = value; }
        public long? ShipId { get => this["_shipId"]; set => this["_shipId"] = value; }

        public DateTime Timestamp { get => this["timestamp"]; set => this["timestamp"] = value; }

        public string Event { get => this["event"]; set => this["event"] = value; }
    }

}