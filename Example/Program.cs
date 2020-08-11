using EliteAPI;
using Somfic.Logging;

using System.IO;
using System.Threading;
using Somfic.Logging.Handlers;

namespace Example
{
    internal class Program
    {
        private static EliteDangerousAPI EliteAPI;

        private static void Main(string[] args)
        {
            Logger.AddHandler(new ConsoleHandler());
            var config = new EliteConfiguration(@"\\DESKTOP-RRQICPT\Users\Lucas\Saved Games\Frontier Developments\Elite Dangerous");
            EliteAPI = new EliteDangerousAPI(config);

            EliteAPI.Start();

            Thread.Sleep(-1);
        }
    }
}