using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using EliteAPI.Events;

namespace EliteAPI.EDSM.Journal
{
    internal interface IEDSMGameStatus
    {
        long? SystemId { get; set; }
        string SystemName { get; set; }
        IReadOnlyList<float> SystemCoordinates { get; set; }
        long? StationId { get; set; }
        string StationName { get; set; }
        long? ShipId { get; set; }
    }

    public class EDSMJournalEntry : Dictionary<string, dynamic>, IEDSMGameStatus, Events.IEvent
    {
        public long? SystemId { get => this["_systemAddress"]; set => this["_systemAddress"] = value; }
        public string SystemName { get => this["_systemName"]; set => this["_systemName"] = value; }
        public IReadOnlyList<float> SystemCoordinates { get => this["_systemCoordinates"]; set => this["_systemCoordinates"] = value; }
        public long? StationId { get => this["_marketId"]; set => this["_marketId"] = value; }
        public string StationName { get => this["_stationName"]; set => this["_stationName"] = value; }
        public long? ShipId { get => this["_shipId"]; set => this["_shipId"] = value; }

        public DateTime Timestamp { get => this["timestamp"]; set => this["timestamp"] = value; }

        public string Event { get => this["event"]; set => this["event"] = value; }

        internal static EDSMJournalEntry FromEliteEvent(EventBase eliteEvent, IEDSMGameStatus gameStatus) 
        {
            var eventJson = JsonConvert.SerializeObject(eliteEvent);
            var journalEvent = JsonConvert.DeserializeObject<EDSMJournalEntry>(eventJson);

            journalEvent.ShipId = gameStatus.ShipId;
            journalEvent.SystemId = gameStatus.SystemId;
            journalEvent.SystemName = gameStatus.SystemName;
            journalEvent.SystemCoordinates = gameStatus.SystemCoordinates;
            journalEvent.StationId = gameStatus.StationId;
            journalEvent.StationName = gameStatus.StationName;

            return journalEvent;
        }
    }

}