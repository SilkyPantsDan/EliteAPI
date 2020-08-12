
using System.Collections.Generic;

namespace EliteAPI.Events
{
    /// <summary>
    /// The interface every event with StarPos implements.
    /// </summary>
    public interface IStarPosInfo
    {
        /// <summary>
        /// The position of the destination star system in light years.
        /// </summary>
         IReadOnlyList<float> StarPos{ get; }
    }
}