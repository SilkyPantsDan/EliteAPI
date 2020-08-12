
namespace EliteAPI.Events
{
    /// <summary>
    /// The interface every event with StarPos implements.
    /// </summary>
    public interface IStarSystemInfo
    {

        /// <summary>
        /// The name of the destination star system.
        /// </summary>
        string StarSystem { get; }

        /// <summary>
        /// The address of the destination star system.aW
        /// </summary>
        long SystemAddress { get; }
    }
}