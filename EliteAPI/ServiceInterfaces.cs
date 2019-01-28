﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EliteAPI.Bindings;
using EliteAPI.Discord;
using EliteAPI.Status;

namespace EliteAPI
{
    public static class ServiceInterfaces
    {
        public interface IEliteDangerousAPI
        {
            //Version info.
            FileVersionInfo Version { get; }
            long MajorVersion { get; }
            long MinorVersion { get; }
            string BuildVersion { get; }

            //Public fields.
            bool IsRunning { get; }
            DirectoryInfo JournalDirectory { get; }
            bool SkipCatchUp { get; }
            EliteAPI.Events.EventHandler Events { get; }
            EliteAPI.Logging.Logger Logger { get; }
            ShipStatus Status { get; }
            ShipCargo Cargo { get; }
            ShipModules Modules { get; }
            UserBindings Bindings { get; }
            CommanderStatus Commander { get; }
            LocationStatus Location { get; }
            StatusWatcher Watcher { get; }

            //Services.
            RichPresenceClient DiscordRichPresence { get; }

            //Methods.
            void Reset();
            void Start();
            void Stop();
        }
    }
}