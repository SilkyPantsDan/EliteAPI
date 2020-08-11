
using System;
using System.Collections.Generic;
using EliteAPI.EDSM;
using EliteAPI.EDSM.Journal;
using EliteAPI.Events;
using EliteAPI.Events.Startup;

namespace EliteAPI.Service.EDSM
{
    public class EDSMService : IService {

        class EDSMGameStatus : IEDSMGameStatus {
            public long? SystemId { get; set; }
            public string SystemName { get; set; }
            public List<float> SystemCoordinates { get; set; }
            public long? StationId { get; set; }
            public string StationName { get; set; }
            public long? ShipId { get; set; }
            public string Commander { get; set; } = "";

            public EDSMGameStatus() {
                this.Clear();
            }

            public void Clear() {
                this.SystemId = null;
                this.SystemName = null;
                this.SystemCoordinates = null;
                this.StationId = null;
                this.StationName = null;
                this.ShipId = null;
            }
        }

        private EDSMConnection edsmConnection;

        private List<string> discardedEvents = new List<string>();
        private EDSMGameStatus gameStatus = new EDSMGameStatus();
        public bool DisableSendEvents { get; set; } = true;

        public void SetConfiguration(EDSMConfiguration configuration) => edsmConnection.Configuration = configuration;

        public EDSMService(EliteDangerousAPI api) {
            
            // Hook into events
            api.Events.AllEvent += HandleAllEvents;
            api.Events.CommanderEvent += (s, e) => { this.gameStatus.Commander = e.Name; };
            
            api.Events.LoadGameEvent += ClearGameStatus;
            api.Events.UndockedEvent += ClearStationStatus;
            
            api.Events.SetUserShipNameEvent += SetShipIdStatus;
            api.Events.ShipyardSwapEvent += SetShipIdStatus;
            api.Events.LoadoutEvent += SetShipIdStatus;
            api.Events.ShipyardBuyEvent += ClearShipIdStatus;

            api.Events.JoinACrewEvent += UpdateCrewStatus;
            api.Events.QuitACrewEvent += UpdateCrewStatus;
            
            api.Events.LocationEvent += UpdateSystemStatus;
            api.Events.DockedEvent += UpdateSystemStatus;
            api.Events.FSDJumpEvent += UpdateSystemStatus;

            api.OnQuit += HandleShutDown;
            api.OnReady += HandleStartUp;
            api.OnReset += HandleReset;

            edsmConnection = new EDSMConnection();
        }

        private void HandleReset(object sender, EventArgs e)
        {
            if (sender is EliteDangerousAPI) {
                var api = sender as EliteDangerousAPI;

                api.Events.AllEvent -= HandleAllEvents;

                api.OnQuit -= HandleShutDown;
                api.OnReady -= HandleStartUp;
                api.OnReset -= HandleReset;
            }
        }

        private void HandleShutDown(object sender, EventArgs e)
        {
        }

        private async void HandleStartUp(object sender, EventArgs e)
        {
            this.discardedEvents = await edsmConnection.GetDiscardedEventTypes();
        }

        private void HandleAllEvents(object sender, EventBase e)
        {
            // Ignore Events if we should not be sending (Crew Mode)
            if (!this.DisableSendEvents) return;

            // Ignore Discarded Events
            if (this.discardedEvents.Contains(e.Event)) return;
        }

        #region EDSM Game Status Events

        private void ClearStationStatus(object sender, UndockedInfo e)
        {
            this.gameStatus.StationId = null;
            this.gameStatus.StationName = null;
        }

        private void ClearShipIdStatus(object sender, ShipyardBuyInfo e)
        {
            this.gameStatus.ShipId = null;
        }

        private void SetShipIdStatus(object sender, EventBase e)
        {
            if (e is SetUserShipNameInfo) {
                this.gameStatus.ShipId = (e as SetUserShipNameInfo).ShipId;
            }
            else if (e is ShipyardSwapInfo) {
                this.gameStatus.ShipId = (e as ShipyardSwapInfo).ShipId;
            }
            else if (e is LoadoutInfo) {
                this.gameStatus.ShipId = (e as LoadoutInfo).ShipId;
            }
            else {
                //TODO: Log warning that event is not handled
            }
        }

        private void ClearGameStatus(object sender, LoadGameInfo e) => this.gameStatus.Clear();

        private void UpdateSystemStatus(object sender, EventBase e)
        {
            throw new NotImplementedException();
        }

        private void UpdateCrewStatus(object sender, ICrewInfo e)
        {
            this.DisableSendEvents = (e.Captain != this.gameStatus.Commander && e is JoinACrewInfo);
            this.gameStatus.Clear();
        }

        #endregion
    }
}