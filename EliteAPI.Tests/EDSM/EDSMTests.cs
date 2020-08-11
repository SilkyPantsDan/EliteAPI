using System;
using System.Collections.Generic;
using Xunit;

using EliteAPI.Tests.Utilities;

namespace EliteAPI.EDSM.Tests
{
    public class EDSMTests
    {
        [Fact]
        public async void EDSM_CheckJournalUploadEvent()
        {
            var config = new EDSMConfiguration("Cmdr Jameson", "apikey-1234", "EliteApi", "1.2.3.4");
            var connection = new EDSMConnection(config, new EchoHttpMessageHandler());

            var response = await connection.UploadJournalEvents(new List<EliteAPI.Events.EventBase>());
            Console.WriteLine($"Response: \n{response}");
        }
    }
}
