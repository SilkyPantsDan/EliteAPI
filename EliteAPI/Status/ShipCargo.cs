﻿namespace EliteAPI.Status
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using System.IO;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ShipCargo
    {
        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("Vessel")]
        public string Vessel { get; set; }

        [JsonProperty("Count")]
        public long Count { get; set; }

        [JsonProperty("Inventory")]
        public List<Item> Inventory { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Name_Localised")]
        public string NameLocalised { get; set; }

        [JsonProperty("Count")]
        public long Count { get; set; }

        [JsonProperty("Stolen")]
        public long Stolen { get; set; }
    }

    public partial class ShipCargo
    {
        public static ShipCargo FromJson(string json) => JsonConvert.DeserializeObject<ShipCargo>(json, EliteAPI.Status.ShipCargoConverter.Settings);
        public static ShipCargo FromFile(FileInfo file, EliteDangerousAPI api)
        {
            if (File.Exists(file.FullName)) { api.Logger.LogError("Could not find Cargo.json."); return new ShipCargo(); }

            //Create a stream from the log file.
            FileStream fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //Create a stream from the file stream.
            StreamReader streamReader = new StreamReader(fileStream);

            //Go through the stream.
            while (!streamReader.EndOfStream)
            {
                //Process this string.
                return FromJson(streamReader.ReadLine());
            }

            api.Logger.LogWarning("Could not update cargo.");

            return new ShipCargo();
        }
    }

    public static class ShipCargoSerializer
    {
        public static string ToJson(this ShipCargo self) => JsonConvert.SerializeObject(self, EliteAPI.Status.ShipCargoConverter.Settings);
    }

    internal static class ShipCargoConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}