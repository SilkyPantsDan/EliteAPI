﻿using System.Collections.Generic;
using EliteAPI.Events;
using Newtonsoft.Json;

namespace EliteAPI.Status.Cargo
{
    public class CargoStatus : EventBase
    {
        internal CargoStatus() { }

        [JsonProperty("Vessel")]
        public string Vessel { get; internal set; }

        [JsonProperty("Count")]
        public long Count { get; internal set; }

        [JsonProperty("Inventory")]
        public IReadOnlyList<Inventory> Inventory { get; internal set; }
    }
}