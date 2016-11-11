using System;

namespace MyAB.Core
{
    /// <summary>
    /// Provides an abstraction around the system clock.
    /// </summary>
    public interface ISystemClock
    {
        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        DateTime Now { get; }
    }
}