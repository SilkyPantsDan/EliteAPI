using System;
using System.Collections.Generic;
using Xunit;

using EliteAPI.Tests.Utilities;
using Newtonsoft.Json;
using System.IO;
using EliteAPI.Bindings;
using EliteAPI.Status.Cargo;
using EliteAPI.Status.Market;
using EliteAPI.Status.Modules;
using EliteAPI.Status.Outfitting;
using EliteAPI.Status.Ship;
using EliteAPI.Status.Shipyard;
using EliteAPI.Service.Discord;
using EliteAPI.Utilities;
using Newtonsoft.Json.Serialization;
using System.Linq;
using Newtonsoft.Json.Linq;
using EliteAPI.Events;

namespace EliteAPI.EDSM.Tests
{
    public class EDSMTests
    {
        string testEvents = @"
        ";

        private T DeserialiseEvent<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Events.JsonSettings.Settings);
        }

        [Fact]
        public async void EDSM_CheckJournalUploadEvent()
        {
            var config = new EDSMConfiguration("Cmdr Jameson", "apikey-1234", "EliteApi", "1.2.3.4");
            var connection = new EDSMConnection(config, new EchoHttpMessageHandler());

            var response = await connection.UploadJournalEvents(new List<EDSM.Journal.EDSMJournalEntry>());
            Console.WriteLine($"Response: \n{response}");
        }

        [Fact]
        public void EDSM_CheckEventConversion()
        {
            var api = new MockEliteApi();
            var service = new Service.EDSM.EDSMService(api);

            var dockedEventJson = @"{
    ""timestamp"": ""2020-08-08T12:18:39Z"",
    ""event"": ""Docked"",
    ""StationName"": ""Qurra Ring"",
    ""StationType"": ""Orbis"",
    ""StarSystem"": ""Eta Serpentis"",
    ""SystemAddress"": 3382320304858,
    ""MarketID"": 3228924672,
    ""StationFaction"": {
        ""Name"": ""Eta Serpentis Crimson Fortune""
    },
    ""StationGovernment"": ""$government_Corporate;"",
    ""StationGovernment_Localised"": ""Corporate"",
    ""StationAllegiance"": ""Federation"",
    ""StationServices"": [
        ""dock"",
        ""autodock"",
        ""commodities"",
        ""contacts"",
        ""exploration"",
        ""missions"",
        ""outfitting"",
        ""crewlounge"",
        ""rearm"",
        ""refuel"",
        ""repair"",
        ""shipyard"",
        ""tuning"",
        ""engineer"",
        ""missionsgenerated"",
        ""flightcontroller"",
        ""stationoperations"",
        ""powerplay"",
        ""searchrescue"",
        ""stationMenu"",
        ""shop""
    ],
    ""StationEconomy"": ""$economy_Industrial;"",
    ""StationEconomy_Localised"": ""Industrial"",
    ""StationEconomies"": [
        {
            ""Name"": ""$economy_Industrial;"",
            ""Name_Localised"": ""Industrial"",
            ""Proportion"": 1.0
        }
    ],
    ""DistFromStarLS"": 2137.633959
}";
            var expectedJson = @"{""_marketId"":3228924672,""_shipId"":null,""_stationName"":""Qurra Ring"",""_systemAddress"":3382320304858,""_systemCoordinates"":null,""_systemName"":""Eta Serpentis"",""ActiveFine"":false,""Anonymously"":false,""CockpitBreach"":false,""DistFromStarLS"":2137.634,""event"":""Docked"",""MarketID"":3228924672,""StarSystem"":""Eta Serpentis"",""StationAllegiance"":""Federation"",""StationEconomies"":[{""Name"":""$economy_Industrial;"",""Name_Localised"":""Industrial"",""Proportion"":1.0}],""StationEconomy"":""$economy_Industrial;"",""StationEconomy_Localised"":""Industrial"",""StationFaction"":{""Name"":""Eta Serpentis Crimson Fortune""},""StationGovernment"":""$government_Corporate;"",""StationGovernment_Localised"":""Corporate"",""StationName"":""Qurra Ring"",""StationServices"":[""dock"",""autodock"",""commodities"",""contacts"",""exploration"",""missions"",""outfitting"",""crewlounge"",""rearm"",""refuel"",""repair"",""shipyard"",""tuning"",""engineer"",""missionsgenerated"",""flightcontroller"",""stationoperations"",""powerplay"",""searchrescue"",""stationMenu"",""shop""],""StationType"":""Orbis"",""SystemAddress"":3382320304858,""timestamp"":""2020-08-08T12:18:39Z"",""Wanted"":false}";
            JournalProcessor.TriggerEvents(dockedEventJson, api);

            Assert.Equal(1, service.BatchedEventCount);

            var journalEvent = service.batchJournalEntries[0];
            var prettyJson = JsonConvert.SerializeObject(journalEvent, JsonSettings.Settings);
            prettyJson = JsonUtility.NormalizeJsonString(prettyJson);
            Console.WriteLine(prettyJson);
            Assert.Equal(prettyJson, expectedJson);
        }
    }

    public class JsonUtility
    {
        public static string NormalizeJsonString(string json)
        {
            // Parse json string into JObject.
            var parsedObject = JObject.Parse(json);

            // Sort properties of JObject.
            var normalizedObject = SortPropertiesAlphabetically(parsedObject);

            // Serialize JObject .
            return JsonConvert.SerializeObject(normalizedObject, JsonSettings.Settings);
        }

        private static JObject SortPropertiesAlphabetically(JObject original)
        {
            var result = new JObject();

            foreach (var property in original.Properties().ToList().OrderBy(p => p.Name))
            {
                var value = property.Value as JObject;

                if (value != null)
                {
                    value = SortPropertiesAlphabetically(value);
                    result.Add(property.Name, value);
                }
                else
                {
                    result.Add(property.Name, property.Value);
                }
            }

            return result;
        }
    }

    internal class MockEliteApi : IEliteDangerousAPI
    {
        public bool IsRunning => true;

        public DirectoryInfo JournalDirectory => throw new NotImplementedException();

        public bool SkipCatchUp => false;

        public Events.EventHandler Events { get; private set; } = new Events.EventHandler();

        public UserBindings Bindings => throw new NotImplementedException();

        public CargoStatus Cargo => throw new NotImplementedException();

        public MarketStatus Market => throw new NotImplementedException();

        public ModulesStatus Modules => throw new NotImplementedException();

        public OutfittingStatus Outfitting => throw new NotImplementedException();

        public ShipStatus Status => throw new NotImplementedException();

        public ShipyardStatus Shipyard => throw new NotImplementedException();

        public DiscordService Discord => throw new NotImplementedException();

        public event EventHandler<Tuple<string, Exception>> OnError;
        public event System.EventHandler OnQuit;
        public event System.EventHandler OnReady;
        public event System.EventHandler OnReset;

        public void ChangeJournal(DirectoryInfo newJournalDirectory)
        {
        }

        public void Reset()
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
